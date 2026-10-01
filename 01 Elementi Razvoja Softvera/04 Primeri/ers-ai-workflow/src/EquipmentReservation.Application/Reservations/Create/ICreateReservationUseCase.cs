namespace EquipmentReservation.Application.Reservations.Create;

public interface ICreateReservationUseCase
{
    Task<CreateReservationResult> HandleAsync(
        CreateReservationCommand command,
        CancellationToken cancellationToken);
}
