using EquipmentReservation.Domain.Inventory;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Domain;

public sealed class InventoryReservationServiceTests
{
    private readonly InventoryReservationService _service = new();

    [Test]
    public void Reserve_WhenStockIsSufficient_ReturnsItemWithReducedStock()
    {
        var item = new InventoryItem(Guid.NewGuid(), Available: 5);

        var result = _service.Reserve(item, quantity: 2);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value?.Available, Is.EqualTo(3));
            Assert.That(item.Available, Is.EqualTo(5));
        }
    }

    [Test]
    public void Reserve_WhenQuantityEqualsStock_ReservesEverything()
    {
        var item = new InventoryItem(Guid.NewGuid(), Available: 2);

        var result = _service.Reserve(item, quantity: 2);

        Assert.That(result.Value?.Available, Is.Zero);
    }

    [Test]
    public void Reserve_WhenStockIsInsufficient_ReturnsStableErrorCode()
    {
        var item = new InventoryItem(Guid.NewGuid(), Available: 2);

        var result = _service.Reserve(item, quantity: 3);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo(InventoryErrorCodes.InsufficientStock));
        }
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void Reserve_WhenQuantityIsNotPositive_ReturnsInvalidQuantity(int quantity)
    {
        var item = new InventoryItem(Guid.NewGuid(), Available: 5);

        var result = _service.Reserve(item, quantity);

        Assert.That(result.Error, Is.EqualTo(InventoryErrorCodes.InvalidQuantity));
    }
}
