namespace EquipmentReservation.Application.Ports.Reservations;

public interface IReservationRequestLock
{
    ValueTask<IAsyncDisposable> AcquireAsync(
        Guid requestId,
        CancellationToken cancellationToken);
}
