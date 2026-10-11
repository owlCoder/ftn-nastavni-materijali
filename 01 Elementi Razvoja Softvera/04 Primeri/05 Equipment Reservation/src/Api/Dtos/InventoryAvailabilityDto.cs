namespace EquipmentReservation.Api.Dtos;

public sealed class InventoryAvailabilityDto
{
    public Guid EquipmentId { get; set; }
    public int Available { get; set; }
}
