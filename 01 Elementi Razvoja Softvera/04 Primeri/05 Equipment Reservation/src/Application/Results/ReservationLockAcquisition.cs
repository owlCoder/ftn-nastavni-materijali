namespace EquipmentReservation.Application.Results;

public sealed class ReservationLockAcquisition
{
    public bool Acquired { get; private init; }
    public IAsyncDisposable Lease { get; private init; } = new NoOpAsyncLease();

    public static ReservationLockAcquisition Success(IAsyncDisposable lease)
    {
        if (lease is null)
            return new ReservationLockAcquisition();

        return new ReservationLockAcquisition { Acquired = true, Lease = lease };
    }
}
