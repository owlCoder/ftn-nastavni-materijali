using EquipmentReservation.Application.Common;
using EquipmentReservation.Application.Ports.Inventory;
using EquipmentReservation.Application.Ports.Reservations;
using EquipmentReservation.Domain.Reservations;

namespace EquipmentReservation.Application.Reservations.Create;

public sealed class CreateReservationHandler : ICreateReservationUseCase
{
    private readonly IValidator<CreateReservationCommand> _validator;
    private readonly IReservationRepository _reservations;
    private readonly IInventoryModule _inventory;
    private readonly IReservationRequestLock _requestLock;
    private readonly IReservationIdGenerator _idGenerator;

    public CreateReservationHandler(
        IValidator<CreateReservationCommand> validator,
        IReservationRepository reservations,
        IInventoryModule inventory,
        IReservationRequestLock requestLock,
        IReservationIdGenerator idGenerator)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _reservations = reservations ?? throw new ArgumentNullException(nameof(reservations));
        _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
        _requestLock = requestLock ?? throw new ArgumentNullException(nameof(requestLock));
        _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    public async Task<CreateReservationResult> HandleAsync(
        CreateReservationCommand command,
        CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(command);
        if (!validation.Success)
            return CreateReservationResult.Invalid(validation.Error);

        await using var requestLease = await _requestLock.AcquireAsync(
            command.RequestId,
            cancellationToken);

        var existing = await _reservations.FindByRequestIdAsync(command.RequestId, cancellationToken);
        if (existing is not null)
            return CreateReservationResult.From(existing, replayed: true);

        var reservation = await ReserveAsync(command, cancellationToken);
        await _reservations.AddAsync(reservation, cancellationToken);
        return CreateReservationResult.From(reservation, replayed: false);
    }

    private async Task<Reservation> ReserveAsync(
        CreateReservationCommand command,
        CancellationToken cancellationToken)
    {
        var reservationId = _idGenerator.NewReservationId();
        var inventoryResult = await _inventory.ReserveAsync(
            new ReserveInventoryRequest(
                command.EquipmentId,
                command.Quantity,
                reservationId),
            cancellationToken);

        return inventoryResult.Success
            ? Reservation.Confirmed(
                reservationId,
                command.RequestId,
                command.EquipmentId,
                command.StudentId,
                command.Quantity)
            : Reservation.Rejected(
                reservationId,
                command.RequestId,
                command.EquipmentId,
                command.StudentId,
                command.Quantity,
                inventoryResult.Error);
    }
}
