namespace EquipmentReservation.Infrastructure.Concurrency;

internal sealed class ReservationRequestLockLease : IAsyncDisposable
{
    private readonly InMemoryReservationRequestLock _owner;
    private readonly Guid _requestId;
    private readonly ReservationRequestLockEntry _entry;

    public ReservationRequestLockLease(InMemoryReservationRequestLock owner, Guid requestId, ReservationRequestLockEntry entry)
    {
        _owner = owner;
        _requestId = requestId;
        _entry = entry;
    }

    private int _disposed;

    public ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 0)
            _owner.Release(_requestId, _entry);

        return ValueTask.CompletedTask;
    }
}
