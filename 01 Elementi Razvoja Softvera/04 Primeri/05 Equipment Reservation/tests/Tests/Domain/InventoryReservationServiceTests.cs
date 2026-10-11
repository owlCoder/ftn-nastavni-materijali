using EquipmentReservation.Domain.Inventory;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Domain;

public sealed class InventoryReservationServiceTests
{
    private readonly InventoryReservationService _service = new();

    [Test]
    public void Reserve_WhenStockIsSufficient_ReturnsItemWithReducedStock()
    {
        var item = new InventoryItem(Guid.NewGuid(), available: 5);

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
        var item = new InventoryItem(Guid.NewGuid(), available: 2);

        var result = _service.Reserve(item, quantity: 2);

        Assert.That(result.Value?.Available, Is.Zero);
    }

    [Test]
    public void Reserve_WhenStockIsInsufficient_ReturnsStableErrorCode()
    {
        var item = new InventoryItem(Guid.NewGuid(), available: 2);

        var result = _service.Reserve(item, quantity: 3);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo(InventoryErrorCodes.InsufficientStock));
        }
    }

    [Test]
    public void Reserve_WithInvalidItem_ReturnsDefaultObjectInsteadOfThrowing()
    {
        var result = _service.Reserve(new InventoryItem(), quantity: 1);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Value.EquipmentId, Is.EqualTo(Guid.Empty));
            Assert.That(result.Error, Is.EqualTo(InventoryErrorCodes.InvalidEquipment));
        }
    }

    [Test]
    public void Reserve_WhenReturningAClass_DoesNotMutateExistingInventoryItem()
    {
        var item = new InventoryItem(Guid.NewGuid(), 4);

        var result = _service.Reserve(item, 2);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Value.EquipmentId, Is.EqualTo(item.EquipmentId));
            Assert.That(result.Value.Available, Is.EqualTo(2));
            Assert.That(item.Available, Is.EqualTo(4));
            Assert.That(result.Value, Is.Not.SameAs(item));
        }
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void Reserve_WhenQuantityIsNotPositive_ReturnsInvalidQuantity(int quantity)
    {
        var item = new InventoryItem(Guid.NewGuid(), available: 5);

        var result = _service.Reserve(item, quantity);

        Assert.That(result.Error, Is.EqualTo(InventoryErrorCodes.InvalidQuantity));
    }
}
