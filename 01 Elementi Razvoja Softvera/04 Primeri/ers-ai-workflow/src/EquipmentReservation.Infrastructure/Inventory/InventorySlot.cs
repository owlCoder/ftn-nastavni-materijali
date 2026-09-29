namespace EquipmentReservation.Infrastructure.Inventory;

internal sealed class InventorySlot(int available)
{
    public int Available { get; set; } = available;
    public object SyncRoot { get; } = new();
}
