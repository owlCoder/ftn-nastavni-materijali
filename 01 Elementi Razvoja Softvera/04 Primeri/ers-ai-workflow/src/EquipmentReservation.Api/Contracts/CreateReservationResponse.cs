namespace EquipmentReservation.Api.Contracts;

public sealed record CreateReservationResponse(
    Guid ReservationId,
    string Outcome,
    string? ErrorCode,
    bool Replayed);
