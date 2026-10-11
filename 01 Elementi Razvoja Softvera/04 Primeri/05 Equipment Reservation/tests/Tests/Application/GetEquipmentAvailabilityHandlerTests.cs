using EquipmentReservation.Application.Inventory.GetAvailability;
using EquipmentReservation.Application.Interfaces.Inventory;
using EquipmentReservation.Domain.Inventory;
using Moq;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Application;

public sealed class GetEquipmentAvailabilityHandlerTests
{
    [Test]
    public async Task Handle_WhenEquipmentExists_ReturnsAvailableQuantity()
    {
        var equipmentId = Guid.NewGuid();
        var handler = new GetEquipmentAvailabilityHandler(ReadModelReturning(7, equipmentId));

        var result = await handler.HandleAsync(new GetEquipmentAvailabilityQuery(equipmentId), CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value.EquipmentId, Is.EqualTo(equipmentId));
            Assert.That(result.Value.Available, Is.EqualTo(7));
        }
    }

    [Test]
    public async Task Handle_WhenEquipmentDoesNotExist_ReturnsStableErrorCode()
    {
        var handler = new GetEquipmentAvailabilityHandler(ReadModelReturning(null, Guid.NewGuid()));

        var result = await handler.HandleAsync(new GetEquipmentAvailabilityQuery(Guid.NewGuid()), CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo(InventoryErrorCodes.EquipmentNotFound));
        }
    }

    [Test]
    public async Task Handle_WhenQueryIsInvalid_ReturnsDefaultAvailability()
    {
        var handler = new GetEquipmentAvailabilityHandler(ReadModelReturning(3, Guid.NewGuid()));

        var result = await handler.HandleAsync(new GetEquipmentAvailabilityQuery(), CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Value.EquipmentId, Is.EqualTo(Guid.Empty));
            Assert.That(result.Error, Is.EqualTo(InventoryErrorCodes.InvalidEquipment));
        }
    }

    private static IInventoryReadModel ReadModelReturning(int? available, Guid equipmentId)
    {
        var readModel = new Mock<IInventoryReadModel>();
        readModel
            .Setup(port => port.GetAvailableAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(available.HasValue ? new EquipmentAvailability(equipmentId, available.Value) : new EquipmentAvailability());
        return readModel.Object;
    }
}
