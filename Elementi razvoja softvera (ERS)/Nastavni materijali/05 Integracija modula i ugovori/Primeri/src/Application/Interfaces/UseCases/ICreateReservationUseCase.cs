using EquipmentReservation.Application.Reservations.Create;

namespace EquipmentReservation.Application.Interfaces.UseCases;

public interface ICreateReservationUseCase
{
    Task<CreateReservationResult> HandleAsync(CreateReservationCommand command, CancellationToken cancellationToken);
}
