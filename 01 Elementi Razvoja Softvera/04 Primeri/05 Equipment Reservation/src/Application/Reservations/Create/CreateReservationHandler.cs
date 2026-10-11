using EquipmentReservation.Application.Inventory.Models;
using EquipmentReservation.Application.Interfaces.Inventory;
using EquipmentReservation.Application.Interfaces.Reservations;
using EquipmentReservation.Application.Interfaces.UseCases;
using EquipmentReservation.Application.Interfaces.Validation;
using EquipmentReservation.Domain.Inventory;
using EquipmentReservation.Domain.Reservations;

namespace EquipmentReservation.Application.Reservations.Create;

public sealed class CreateReservationHandler : ICreateReservationUseCase
{
    private readonly IValidator<CreateReservationCommand> _validator;
    private readonly IReservationRepository _reservations;
    private readonly IInventoryModule _inventory;
    private readonly IReservationRequestLock _requestLock;
    private readonly IReservationIdGenerator _idGenerator;

    public CreateReservationHandler(IValidator<CreateReservationCommand> validator, IReservationRepository reservations, IInventoryModule inventory, IReservationRequestLock requestLock, IReservationIdGenerator idGenerator)
    {
        _validator = validator;
        _reservations = reservations;
        _inventory = inventory;
        _requestLock = requestLock;
        _idGenerator = idGenerator;
    }

    public async Task<CreateReservationResult> HandleAsync(CreateReservationCommand command, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(command);
        if (!validation.Success)
            return CreateReservationResult.Invalid(validation.Error);
        if (cancellationToken.IsCancellationRequested)
            return CreateReservationResult.Invalid(CreateReservationErrorCodes.RequestCancelled);

        var acquisition = await _requestLock.AcquireAsync(command.RequestId, cancellationToken);
        if (!acquisition.Acquired)
            return CreateReservationResult.Invalid(CreateReservationErrorCodes.RequestCancelled);

        await using var lease = acquisition.Lease;
        if (cancellationToken.IsCancellationRequested)
            return CreateReservationResult.Invalid(CreateReservationErrorCodes.RequestCancelled);

        var existing = await _reservations.FindByRequestIdAsync(command.RequestId, cancellationToken);
        if (existing.Id != Guid.Empty)
        {
            if (existing.EquipmentId != command.EquipmentId || existing.StudentId != command.StudentId || existing.Quantity != command.Quantity)
                return CreateReservationResult.Invalid(CreateReservationErrorCodes.RequestPayloadMismatch);

            return CreateReservationResult.From(existing, replayed: true);
        }

        var reservationId = _idGenerator.NewReservationId();
        if (reservationId == Guid.Empty)
            return CreateReservationResult.Invalid(CreateReservationErrorCodes.PersistenceFailed);

        var inventoryRequest = new ReserveInventoryRequest(command.EquipmentId, command.Quantity, reservationId);
        var stock = await _inventory.ReserveAsync(inventoryRequest, cancellationToken);
        var reservation = stock.Success
            ? Reservation.Confirmed(reservationId, command.RequestId, command.EquipmentId, command.StudentId, command.Quantity)
            : Reservation.Rejected(reservationId, command.RequestId, command.EquipmentId, command.StudentId, command.Quantity, stock.Error);

        // After stock changes, complete persistence even if the caller disconnects.
        var saved = await _reservations.AddAsync(reservation, CancellationToken.None);
        if (!saved)
        {
            if (stock.Success)
            {
                var rollback = await _inventory.ReleaseAsync(inventoryRequest, CancellationToken.None);
                if (!rollback.Success)
                    return CreateReservationResult.Invalid(CreateReservationErrorCodes.CompensationFailed);
            }

            return CreateReservationResult.Invalid(CreateReservationErrorCodes.PersistenceFailed);
        }

        return CreateReservationResult.From(reservation, replayed: false);
    }
}
