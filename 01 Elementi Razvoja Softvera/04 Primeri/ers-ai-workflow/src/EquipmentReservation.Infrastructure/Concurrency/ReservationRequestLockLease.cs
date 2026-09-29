namespace EquipmentReservation.Infrastructure.Concurrency;

internal sealed class ReservationRequestLockLease(
    InMemoryReservationRequestLock owner,
    Guid requestId,
    ReservationRequestLockEntry entry) : IAsyncDisposable
{
    private int _disposed;

    public ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 0)
            owner.Release(requestId, entry);

        return ValueTask.CompletedTask;
    }
}
