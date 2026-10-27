namespace EquipmentReservation.Domain.Inventory;

public sealed class EquipmentAvailability
{
    public EquipmentAvailability() { }

    public EquipmentAvailability(Guid equipmentId, int available)
    {
        EquipmentId = equipmentId;
        Available = available;
    }

    public Guid EquipmentId { get; init; }
    public int Available { get; init; }
}
