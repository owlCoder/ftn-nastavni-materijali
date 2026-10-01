using EquipmentReservation.Domain.Inventory;

namespace EquipmentReservation.Infrastructure.Inventory;

internal sealed class InventorySlot(InventoryItem item)
{
    public InventoryItem Item { get; set; } = item;
    public object SyncRoot { get; } = new();
}
