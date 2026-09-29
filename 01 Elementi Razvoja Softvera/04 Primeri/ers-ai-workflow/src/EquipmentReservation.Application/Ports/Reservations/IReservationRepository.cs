using EquipmentReservation.Domain.Reservations;

namespace EquipmentReservation.Application.Ports.Reservations;

public interface IReservationRepository
{
    Task<Reservation?> FindByRequestIdAsync(
        Guid requestId,
        CancellationToken cancellationToken);

    Task AddAsync(
        Reservation reservation,
        CancellationToken cancellationToken);
}
