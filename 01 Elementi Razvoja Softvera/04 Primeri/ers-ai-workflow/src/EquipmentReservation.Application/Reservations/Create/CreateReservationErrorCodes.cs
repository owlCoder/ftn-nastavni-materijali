namespace EquipmentReservation.Application.Reservations.Create;

public static class CreateReservationErrorCodes
{
    public const string RequestIdRequired = "RequestIdRequired";
    public const string EquipmentIdRequired = "EquipmentIdRequired";
    public const string StudentIdRequired = "StudentIdRequired";
    public const string QuantityMustBePositive = "QuantityMustBePositive";
}
