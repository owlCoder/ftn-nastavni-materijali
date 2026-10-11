using EquipmentReservation.Domain.Reservations;

namespace EquipmentReservation.Application.Interfaces.Reservations;

public interface IReservationRepository
{
    Task<Reservation> FindByRequestIdAsync(Guid requestId, CancellationToken cancellationToken);
    Task<bool> AddAsync(Reservation reservation, CancellationToken cancellationToken);
}
