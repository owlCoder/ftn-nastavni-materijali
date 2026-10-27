namespace EquipmentReservation.Domain.Inventory;

public sealed class InventoryItem
{
    public InventoryItem() { }

    public InventoryItem(Guid equipmentId, int available)
    {
        EquipmentId = equipmentId;
        Available = available;
    }

    public Guid EquipmentId { get; init; }
    public int Available { get; init; }
}
