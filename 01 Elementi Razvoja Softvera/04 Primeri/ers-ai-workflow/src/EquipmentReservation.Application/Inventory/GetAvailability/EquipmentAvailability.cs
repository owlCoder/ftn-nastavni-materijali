namespace EquipmentReservation.Application.Inventory.GetAvailability;

public sealed record EquipmentAvailability(
    Guid EquipmentId,
    int Available);
