using EquipmentReservation.Domain.Inventory;

namespace EquipmentReservation.Infrastructure.Inventory;

internal sealed class InventorySlot
{
    public InventorySlot(InventoryItem item)
    {
        Item = item;
        Capacity = item.Available;
    }

    public InventoryItem Item { get; set; }
    public int Capacity { get; }
    public Dictionary<Guid, int> Reservations { get; } = new();
    public object SyncRoot { get; } = new();
}
