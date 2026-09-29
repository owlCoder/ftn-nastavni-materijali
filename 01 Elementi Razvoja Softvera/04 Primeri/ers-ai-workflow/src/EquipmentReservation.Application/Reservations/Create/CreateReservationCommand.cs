namespace EquipmentReservation.Application.Reservations.Create;

public sealed record CreateReservationCommand(
    Guid RequestId,
    Guid EquipmentId,
    Guid StudentId,
    int Quantity);
