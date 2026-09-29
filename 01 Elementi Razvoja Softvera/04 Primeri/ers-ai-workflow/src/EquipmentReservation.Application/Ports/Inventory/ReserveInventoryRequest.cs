namespace EquipmentReservation.Application.Ports.Inventory;

public sealed record ReserveInventoryRequest(
    Guid EquipmentId,
    int Quantity,
    Guid ReservationId);
