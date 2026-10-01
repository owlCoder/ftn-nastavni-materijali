using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.Domain.Inventory;
using EquipmentReservation.Domain.Reservations;
using EquipmentReservation.Infrastructure.Concurrency;
using EquipmentReservation.Infrastructure.Identity;
using EquipmentReservation.Infrastructure.Inventory;
using EquipmentReservation.Infrastructure.Persistence;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Integration;

public sealed class ReservationFlowTests
{
    private static readonly Guid EquipmentId = Guid.NewGuid();

    private InMemoryInventoryModule _inventory = null!;
    private InMemoryReservationRepository _repository = null!;
    private CreateReservationHandler _handler = null!;

    [Test]
    public async Task CreateReservation_WhenInventoryIsAvailable_ConfirmsAndDecrementsInventory()
    {
        ComposeSystem(available: 5);

        var result = await _handler.HandleAsync(Command(quantity: 2), CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(CreateReservationOutcome.Confirmed));
            Assert.That(result.ErrorCode, Is.Null);
            Assert.That(result.Replayed, Is.False);
            Assert.That(await AvailableAsync(), Is.EqualTo(3));
        }
    }

    [Test]
    public async Task CreateReservation_WhenInventoryIsInsufficient_RejectsWithoutChangingInventory()
    {
        ComposeSystem(available: 2);
        var command = Command(quantity: 3);

        var result = await _handler.HandleAsync(command, CancellationToken.None);
        var stored = await _repository.FindByRequestIdAsync(
            command.RequestId,
            CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(CreateReservationOutcome.Rejected));
            Assert.That(result.ErrorCode, Is.EqualTo(InventoryErrorCodes.InsufficientStock));
            Assert.That(stored?.Status, Is.EqualTo(ReservationStatus.Rejected));
            Assert.That(await AvailableAsync(), Is.EqualTo(2));
        }
    }

    [Test]
    public async Task CreateReservation_WhenEquipmentDoesNotExist_ReturnsClearRejection()
    {
        ComposeSystem(available: 5);
        var command = Command(quantity: 1) with { EquipmentId = Guid.NewGuid() };

        var result = await _handler.HandleAsync(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(CreateReservationOutcome.Rejected));
            Assert.That(result.ErrorCode, Is.EqualTo(InventoryErrorCodes.EquipmentNotFound));
        }
    }

    [Test]
    public async Task CreateReservation_WhenRequestIsRepeated_IsIdempotent()
    {
        ComposeSystem(available: 5);
        var command = Command(quantity: 2);

        var first = await _handler.HandleAsync(command, CancellationToken.None);
        var replay = await _handler.HandleAsync(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(first.Outcome, Is.EqualTo(CreateReservationOutcome.Confirmed));
            Assert.That(replay.ReservationId, Is.EqualTo(first.ReservationId));
            Assert.That(replay.Replayed, Is.True);
            Assert.That(await AvailableAsync(), Is.EqualTo(3));
        }
    }

    [Test]
    public async Task CreateReservation_WhenSameRequestRunsConcurrently_ReservesOnlyOnce()
    {
        ComposeSystem(available: 5);
        var command = Command(quantity: 2);

        var results = await Task.WhenAll(
            Enumerable.Range(0, 12)
                .Select(_ => _handler.HandleAsync(command, CancellationToken.None)));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(
                results.Select(result => result.ReservationId).Distinct().Count(),
                Is.EqualTo(1));
            Assert.That(results.Count(result => !result.Replayed), Is.EqualTo(1));
            Assert.That(await AvailableAsync(), Is.EqualTo(3));
        }
    }

    [Test]
    public async Task CreateReservation_WhenDifferentRequestsCompete_NeverOversellsInventory()
    {
        ComposeSystem(available: 5);

        var results = await Task.WhenAll(
            Enumerable.Range(0, 12)
                .Select(_ => _handler.HandleAsync(Command(quantity: 1), CancellationToken.None)));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(
                results.Count(result => result.Outcome == CreateReservationOutcome.Confirmed),
                Is.EqualTo(5));
            Assert.That(await AvailableAsync(), Is.Zero);
        }
    }

    private void ComposeSystem(int available)
    {
        _inventory = new InMemoryInventoryModule(
            new InventoryReservationService(),
            [new InventoryItem(EquipmentId, available)]);
        _repository = new InMemoryReservationRepository();
        _handler = new CreateReservationHandler(
            new CreateReservationCommandValidator(),
            _repository,
            _inventory,
            new InMemoryReservationRequestLock(),
            new GuidReservationIdGenerator());
    }

    private Task<int?> AvailableAsync() =>
        _inventory.GetAvailableAsync(EquipmentId, CancellationToken.None);

    private static CreateReservationCommand Command(int quantity) =>
        new(Guid.NewGuid(), EquipmentId, Guid.NewGuid(), quantity);
}
