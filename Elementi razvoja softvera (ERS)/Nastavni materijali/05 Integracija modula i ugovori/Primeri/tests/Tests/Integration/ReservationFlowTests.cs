using EquipmentReservation.Application.Inventory.Models;
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
            Assert.That(result.ErrorCode, Is.Empty);
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
            Assert.That(stored.Status, Is.EqualTo(ReservationStatus.Rejected));
            Assert.That(await AvailableAsync(), Is.EqualTo(2));
        }
    }

    [Test]
    public async Task CreateReservation_WhenEquipmentDoesNotExist_ReturnsClearRejection()
    {
        ComposeSystem(available: 5);
        var command = new CreateReservationCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);

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

    [Test]
    public async Task CreateReservation_WhenSameRequestHasDifferentPayload_DoesNotReuseTheReservation()
    {
        ComposeSystem(available: 5);
        var first = Command(quantity: 1);

        await _handler.HandleAsync(first, CancellationToken.None);
        var second = await _handler.HandleAsync(new CreateReservationCommand(first.RequestId, first.EquipmentId, first.StudentId, 3), CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(second.Outcome, Is.EqualTo(CreateReservationOutcome.Invalid));
            Assert.That(second.ErrorCode, Is.EqualTo(CreateReservationErrorCodes.RequestPayloadMismatch));
            Assert.That(second.ReservationId, Is.EqualTo(Guid.Empty));
            Assert.That(await AvailableAsync(), Is.EqualTo(4));
        }
    }

    [Test]
    public async Task CreateReservation_WhenRequestIsCancelled_DoesNotChangeStock()
    {
        ComposeSystem(available: 5);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        var result = await _handler.HandleAsync(Command(quantity: 2), cancellation.Token);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.ErrorCode, Is.EqualTo(CreateReservationErrorCodes.RequestCancelled));
            Assert.That(await AvailableAsync(), Is.EqualTo(5));
        }
    }

    [Test]
    public void Inventory_WhenInitialItemsContainDuplicateId_ReportsProblemWithoutThrowing()
    {
        var inventory = new InMemoryInventoryModule(new InventoryReservationService(),
            [new InventoryItem(EquipmentId, 2), new InventoryItem(EquipmentId, 5)]);

        Assert.That(inventory.InitializationErrors, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task Inventory_WhenReservationIsReleased_RestoresStockOnlyOnce()
    {
        ComposeSystem(available: 5);
        var request = new EquipmentReservation.Application.Inventory.Models.ReserveInventoryRequest(EquipmentId, 2, Guid.NewGuid());
        await _inventory.ReserveAsync(request, CancellationToken.None);
        var first = await _inventory.ReleaseAsync(request, CancellationToken.None);
        var second = await _inventory.ReleaseAsync(request, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(first.Success, Is.True);
            Assert.That(second.Success, Is.False);
            Assert.That(await AvailableAsync(), Is.EqualTo(5));
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

    private async Task<int> AvailableAsync() =>
        (await _inventory.GetAvailableAsync(EquipmentId, CancellationToken.None)).Available;

    private static CreateReservationCommand Command(int quantity) =>
        new(Guid.NewGuid(), EquipmentId, Guid.NewGuid(), quantity);
}
