namespace Odp.Vezba05.Domain.ReadModels;

public sealed class FreshnessPolicy
{
    private readonly TimeSpan _staleAfter;

    public FreshnessPolicy(TimeSpan staleAfter)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(staleAfter, TimeSpan.Zero);
        _staleAfter = staleAfter;
    }

    public bool IsStale(StationView view, DateTimeOffset now)
    {
        ArgumentNullException.ThrowIfNull(view);

        return now - view.LastMeasuredAt > _staleAfter;
    }
}
