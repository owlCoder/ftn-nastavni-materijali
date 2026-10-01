namespace EquipmentReservation.Api.Contracts;

public sealed record InventoryAvailabilityResponse(
    Guid EquipmentId,
    int Available);
