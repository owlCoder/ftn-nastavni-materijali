using EquipmentReservation.Application.Results;

namespace EquipmentReservation.Application.Interfaces.Reservations;

public interface IReservationRequestLock
{
    ValueTask<ReservationLockAcquisition> AcquireAsync(Guid requestId, CancellationToken cancellationToken);
}
