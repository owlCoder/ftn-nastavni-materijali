using EquipmentReservation.Application.Inventory.GetAvailability;
using EquipmentReservation.Application.Ports.Inventory;
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
        var handler = new GetEquipmentAvailabilityHandler(ReadModelReturning(7));

        var result = await handler.HandleAsync(
            new GetEquipmentAvailabilityQuery(equipmentId),
            CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(new EquipmentAvailability(equipmentId, 7)));
        }
    }

    [Test]
    public async Task Handle_WhenEquipmentDoesNotExist_ReturnsStableErrorCode()
    {
        var handler = new GetEquipmentAvailabilityHandler(ReadModelReturning(null));

        var result = await handler.HandleAsync(
            new GetEquipmentAvailabilityQuery(Guid.NewGuid()),
            CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo(InventoryErrorCodes.EquipmentNotFound));
        }
    }

    private static IInventoryReadModel ReadModelReturning(int? available)
    {
        var readModel = new Mock<IInventoryReadModel>();
        readModel
            .Setup(port => port.GetAvailableAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(available);
        return readModel.Object;
    }
}
