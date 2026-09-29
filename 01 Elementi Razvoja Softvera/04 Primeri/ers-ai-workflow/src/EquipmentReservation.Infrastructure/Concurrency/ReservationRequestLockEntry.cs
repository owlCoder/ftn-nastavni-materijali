namespace EquipmentReservation.Infrastructure.Concurrency;

internal sealed class ReservationRequestLockEntry
{
    public object SyncRoot { get; } = new();
    public SemaphoreSlim Semaphore { get; } = new(1, 1);
    public int Users { get; set; }
    public bool Removed { get; set; }
}
