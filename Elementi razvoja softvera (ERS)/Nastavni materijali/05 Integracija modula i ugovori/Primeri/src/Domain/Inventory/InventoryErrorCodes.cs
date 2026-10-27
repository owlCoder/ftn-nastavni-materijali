namespace EquipmentReservation.Domain.Inventory;

public static class InventoryErrorCodes
{
    public const string EquipmentNotFound = "EquipmentNotFound";
    public const string InvalidQuantity = "InvalidQuantity";
    public const string InvalidEquipment = "InvalidEquipment";
    public const string InsufficientStock = "InsufficientStock";
    public const string DuplicateReservation = "DuplicateReservation";
    public const string InvalidRelease = "InvalidRelease";
    public const string RequestCancelled = "RequestCancelled";
}
