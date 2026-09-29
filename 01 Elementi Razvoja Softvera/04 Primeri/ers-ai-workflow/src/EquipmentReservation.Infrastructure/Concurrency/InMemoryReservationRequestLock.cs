using System.Collections.Concurrent;
using EquipmentReservation.Application.Ports.Reservations;

namespace EquipmentReservation.Infrastructure.Concurrency;

public sealed class InMemoryReservationRequestLock : IReservationRequestLock
{
    private readonly ConcurrentDictionary<Guid, ReservationRequestLockEntry> _entries = new();

    public async ValueTask<IAsyncDisposable> AcquireAsync(
        Guid requestId,
        CancellationToken cancellationToken)
    {
        ReservationRequestLockEntry entry;

        while (true)
        {
            entry = _entries.GetOrAdd(requestId, static _ => new ReservationRequestLockEntry());
            lock (entry.SyncRoot)
            {
                if (entry.Removed)
                    continue;

                entry.Users++;
                break;
            }
        }

        try
        {
            await entry.Semaphore.WaitAsync(cancellationToken);
            return new ReservationRequestLockLease(this, requestId, entry);
        }
        catch
        {
            RemoveUser(requestId, entry);
            throw;
        }
    }

    internal void Release(Guid requestId, ReservationRequestLockEntry entry)
    {
        entry.Semaphore.Release();
        RemoveUser(requestId, entry);
    }

    private void RemoveUser(Guid requestId, ReservationRequestLockEntry entry)
    {
        lock (entry.SyncRoot)
        {
            entry.Users--;
            if (entry.Users != 0)
                return;

            entry.Removed = true;
            _entries.TryRemove(
                new KeyValuePair<Guid, ReservationRequestLockEntry>(requestId, entry));
        }
    }
}
