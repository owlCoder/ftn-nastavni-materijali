namespace EquipmentReservation.Application.Inventory.Models;

public sealed class ReserveInventoryRequest
{
    public ReserveInventoryRequest() { }

    public ReserveInventoryRequest(Guid equipmentId, int quantity, Guid reservationId)
    {
        EquipmentId = equipmentId;
        Quantity = quantity;
        ReservationId = reservationId;
    }

    public Guid EquipmentId { get; init; }
    public int Quantity { get; init; }
    public Guid ReservationId { get; init; }
}
