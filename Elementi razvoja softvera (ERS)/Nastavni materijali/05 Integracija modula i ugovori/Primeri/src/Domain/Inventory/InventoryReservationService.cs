using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Domain.Inventory;

public sealed class InventoryReservationService
{
    public Result<InventoryItem> Reserve(InventoryItem item, int quantity)
    {
        if (item is null || item.EquipmentId == Guid.Empty || item.Available < 0)
            return Result<InventoryItem>.Fail(InventoryErrorCodes.InvalidEquipment, new InventoryItem());

        if (quantity <= 0)
            return Result<InventoryItem>.Fail(InventoryErrorCodes.InvalidQuantity, new InventoryItem());

        if (item.Available < quantity)
            return Result<InventoryItem>.Fail(InventoryErrorCodes.InsufficientStock, new InventoryItem());

        return Result<InventoryItem>.Ok(new InventoryItem(item.EquipmentId, item.Available - quantity));
    }
}
