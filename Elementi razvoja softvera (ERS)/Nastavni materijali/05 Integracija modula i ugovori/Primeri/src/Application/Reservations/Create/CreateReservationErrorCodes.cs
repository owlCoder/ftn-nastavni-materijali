namespace EquipmentReservation.Application.Reservations.Create;

public static class CreateReservationErrorCodes
{
    public const string RequestIdRequired = "RequestIdRequired";
    public const string EquipmentIdRequired = "EquipmentIdRequired";
    public const string StudentIdRequired = "StudentIdRequired";
    public const string QuantityMustBePositive = "QuantityMustBePositive";
    public const string RequestPayloadMismatch = "RequestPayloadMismatch";
    public const string RequestCancelled = "RequestCancelled";
    public const string PersistenceFailed = "ReservationPersistenceFailed";
    public const string CompensationFailed = "InventoryCompensationFailed";
}
