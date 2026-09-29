using EquipmentReservation.Application.Ports.Inventory;
using EquipmentReservation.Application.Ports.Reservations;
using EquipmentReservation.Domain.Reservations;

namespace EquipmentReservation.Application.Reservations.Create;

public sealed class CreateReservationHandler
{
    private readonly IReservationRepository _reservations;
    private readonly IInventoryModule _inventory;
    private readonly IReservationRequestLock _requestLock;

    public CreateReservationHandler(
        IReservationRepository reservations,
        IInventoryModule inventory,
        IReservationRequestLock requestLock)
    {
        _reservations = reservations ?? throw new ArgumentNullException(nameof(reservations));
        _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
        _requestLock = requestLock ?? throw new ArgumentNullException(nameof(requestLock));
    }

    public async Task<CreateReservationResult> HandleAsync(
        CreateReservationCommand command,
        CancellationToken cancellationToken)
    {
        CreateReservationCommandValidator.ValidateAndThrow(command);
        await using var requestLease = await _requestLock.AcquireAsync(
            command.RequestId,
            cancellationToken);

        var existing = await _reservations.FindByRequestIdAsync(command.RequestId, cancellationToken);
        if (existing is not null)
            return Map(existing, replayed: true);

        var reservationId = Guid.NewGuid();
        var inventoryResult = await _inventory.ReserveAsync(
            new ReserveInventoryRequest(
                command.EquipmentId,
                command.Quantity,
                reservationId),
            cancellationToken);

        var status = inventoryResult.Success
            ? ReservationStatus.Confirmed
            : ReservationStatus.Rejected;
        var rejectionReason = inventoryResult.Success
            ? null
            : inventoryResult.ErrorCode ?? "InventoryRejected";

        var reservation = new Reservation(
            reservationId,
            command.RequestId,
            command.EquipmentId,
            command.StudentId,
            command.Quantity,
            status,
            rejectionReason);

        await _reservations.AddAsync(reservation, cancellationToken);
        return Map(reservation, replayed: false);
    }

    private static CreateReservationResult Map(Reservation reservation, bool replayed) =>
        new(
            reservation.Id,
            reservation.Status switch
            {
                ReservationStatus.Confirmed => CreateReservationOutcome.Confirmed,
                ReservationStatus.Rejected => CreateReservationOutcome.Rejected,
                _ => throw new InvalidOperationException("A persisted reservation must have a final status.")
            },
            reservation.RejectionReason,
            replayed);
}
