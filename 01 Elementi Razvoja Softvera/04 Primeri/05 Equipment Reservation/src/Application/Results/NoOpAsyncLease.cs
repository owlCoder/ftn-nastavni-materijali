namespace EquipmentReservation.Application.Results;

internal sealed class NoOpAsyncLease : IAsyncDisposable
{
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
