using EquipmentReservation.Domain.Reservations;

namespace EquipmentReservation.Application.Reservations.Create;

public sealed record CreateReservationResult(
    CreateReservationOutcome Outcome,
    Guid? ReservationId,
    string? ErrorCode,
    bool Replayed)
{
    public static CreateReservationResult Invalid(string errorCode) =>
        new(CreateReservationOutcome.Invalid, null, errorCode, false);

    public static CreateReservationResult From(Reservation reservation, bool replayed) =>
        new(
            reservation.Status == ReservationStatus.Confirmed
                ? CreateReservationOutcome.Confirmed
                : CreateReservationOutcome.Rejected,
            reservation.Id,
            reservation.RejectionReason,
            replayed);
}
