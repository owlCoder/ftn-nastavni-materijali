namespace EquipmentReservation.Domain.Inventory;

public sealed record InventoryItem(
    Guid EquipmentId,
    int Available);
