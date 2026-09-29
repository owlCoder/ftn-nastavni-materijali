namespace EquipmentReservation.Application.Reservations.Create;

public sealed record CreateReservationResult(
    Guid ReservationId,
    CreateReservationOutcome Outcome,
    string? ErrorCode,
    bool Replayed);
