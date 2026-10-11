using EquipmentReservation.Domain.Reservations;

namespace EquipmentReservation.Application.Reservations.Create;

public sealed class CreateReservationResult
{
    public CreateReservationOutcome Outcome { get; private init; } = CreateReservationOutcome.Invalid;
    public Guid ReservationId { get; private init; }
    public string ErrorCode { get; private init; } = string.Empty;
    public bool Replayed { get; private init; }

    public static CreateReservationResult Invalid(string errorCode) => new()
    {
        Outcome = CreateReservationOutcome.Invalid,
        ErrorCode = errorCode
    };

    public static CreateReservationResult From(Reservation reservation, bool replayed)
    {
        if (reservation is null || reservation.Id == Guid.Empty || reservation.Status == ReservationStatus.Unknown)
            return Invalid(CreateReservationErrorCodes.PersistenceFailed);

        return new CreateReservationResult
        {
            Outcome = reservation.Status == ReservationStatus.Confirmed
                ? CreateReservationOutcome.Confirmed : CreateReservationOutcome.Rejected,
            ReservationId = reservation.Id,
            ErrorCode = reservation.RejectionReason,
            Replayed = replayed
        };
    }
}
