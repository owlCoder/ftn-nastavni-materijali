namespace EquipmentReservation.Application.Ports.Inventory;

public sealed record ReserveInventoryResult(
    bool Success,
    string? ErrorCode);
