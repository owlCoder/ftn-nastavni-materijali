namespace EquipmentReservation.Application.Inventory.GetAvailability;

public sealed class GetEquipmentAvailabilityQuery
{
    public GetEquipmentAvailabilityQuery() { }

    public GetEquipmentAvailabilityQuery(Guid equipmentId)
    {
        EquipmentId = equipmentId;
    }

    public Guid EquipmentId { get; init; }
}
