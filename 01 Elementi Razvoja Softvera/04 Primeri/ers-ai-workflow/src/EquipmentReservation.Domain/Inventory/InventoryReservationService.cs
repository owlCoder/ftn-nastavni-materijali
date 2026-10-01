using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Domain.Inventory;

public sealed class InventoryReservationService
{
    public Result<InventoryItem> Reserve(InventoryItem item, int quantity)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (quantity <= 0)
            return Result<InventoryItem>.Fail(InventoryErrorCodes.InvalidQuantity);
        if (item.Available < quantity)
            return Result<InventoryItem>.Fail(InventoryErrorCodes.InsufficientStock);

        return Result<InventoryItem>.Ok(item with { Available = item.Available - quantity });
    }
}
