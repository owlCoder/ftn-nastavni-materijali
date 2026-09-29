using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.Domain.Reservations;
using EquipmentReservation.Infrastructure.Concurrency;
using EquipmentReservation.Infrastructure.Inventory;
using EquipmentReservation.Infrastructure.Persistence;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Application;

public sealed class CreateReservationHandlerTests
{
    [Test]
    public async Task CreateReservation_WhenInventoryIsAvailable_ConfirmsAndDecrementsInventory()
    {
        var system = CreateSystem(available: 5);
        var command = CreateCommand(system.EquipmentId, quantity: 2);

        var result = await system.Handler.HandleAsync(command, CancellationToken.None);
        var available = await system.Inventory.GetAvailableAsync(
            command.EquipmentId,
            CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(CreateReservationOutcome.Confirmed));
            Assert.That(result.ErrorCode, Is.Null);
            Assert.That(result.Replayed, Is.False);
            Assert.That(available, Is.EqualTo(3));
        }
    }

    [Test]
    public async Task CreateReservation_WhenInventoryIsInsufficient_RejectsWithoutChangingInventory()
    {
        var system = CreateSystem(available: 2);
        var command = CreateCommand(system.EquipmentId, quantity: 3);

        var result = await system.Handler.HandleAsync(command, CancellationToken.None);
        var available = await system.Inventory.GetAvailableAsync(
            command.EquipmentId,
            CancellationToken.None);
        var stored = await system.Repository.FindByRequestIdAsync(
            command.RequestId,
            CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(CreateReservationOutcome.Rejected));
            Assert.That(result.ErrorCode, Is.EqualTo("InsufficientStock"));
            Assert.That(available, Is.EqualTo(2));
            Assert.That(stored?.Status, Is.EqualTo(ReservationStatus.Rejected));
        }
    }

    [Test]
    public async Task CreateReservation_WhenEquipmentDoesNotExist_ReturnsClearRejection()
    {
        var inventory = new InMemoryInventoryModule();
        var handler = new CreateReservationHandler(
            new InMemoryReservationRepository(),
            inventory,
            new InMemoryReservationRequestLock());

        var result = await handler.HandleAsync(
            CreateCommand(Guid.NewGuid(), quantity: 1),
            CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(CreateReservationOutcome.Rejected));
            Assert.That(result.ErrorCode, Is.EqualTo("EquipmentNotFound"));
        }
    }

    [Test]
    public void CreateReservation_WhenCommandIsInvalid_RejectsBeforeCallingInventory()
    {
        var system = CreateSystem(available: 5);
        var command = CreateCommand(system.EquipmentId, quantity: 0);

        Assert.That(
            async () => await system.Handler.HandleAsync(command, CancellationToken.None),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public async Task CreateReservation_WhenRequestIsRepeated_IsIdempotent()
    {
        var system = CreateSystem(available: 5);
        var command = CreateCommand(system.EquipmentId, quantity: 2);

        var first = await system.Handler.HandleAsync(command, CancellationToken.None);
        var replay = await system.Handler.HandleAsync(command, CancellationToken.None);
        var available = await system.Inventory.GetAvailableAsync(
            command.EquipmentId,
            CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(first.Outcome, Is.EqualTo(CreateReservationOutcome.Confirmed));
            Assert.That(replay.ReservationId, Is.EqualTo(first.ReservationId));
            Assert.That(replay.Replayed, Is.True);
            Assert.That(available, Is.EqualTo(3));
        }
    }

    [Test]
    public async Task CreateReservation_WhenSameRequestRunsConcurrently_ReservesOnlyOnce()
    {
        var system = CreateSystem(available: 5);
        var command = CreateCommand(system.EquipmentId, quantity: 2);

        var results = await Task.WhenAll(
            Enumerable.Range(0, 12)
                .Select(_ => system.Handler.HandleAsync(command, CancellationToken.None)));
        var available = await system.Inventory.GetAvailableAsync(
            command.EquipmentId,
            CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(
                results.Select(result => result.ReservationId).Distinct().Count(),
                Is.EqualTo(1));
            Assert.That(results.Count(result => !result.Replayed), Is.EqualTo(1));
            Assert.That(available, Is.EqualTo(3));
        }
    }

    private static TestSystem CreateSystem(int available)
    {
        var equipmentId = Guid.NewGuid();
        var inventory = new InMemoryInventoryModule();
        inventory.Seed(equipmentId, available);
        var repository = new InMemoryReservationRepository();

        return new TestSystem(
            new CreateReservationHandler(
                repository,
                inventory,
                new InMemoryReservationRequestLock()),
            inventory,
            repository,
            equipmentId);
    }

    private static CreateReservationCommand CreateCommand(Guid equipmentId, int quantity) =>
        new(
            Guid.NewGuid(),
            equipmentId,
            Guid.NewGuid(),
            quantity);

}
