using Odp.Vezba05.Application.Ports;
using Odp.Vezba05.Domain.ReadModels;

namespace Odp.Vezba05.Application.Queries;

public sealed class GetStationStatusHandler(
    IStationViewStore views,
    FreshnessPolicy freshness,
    IClock clock) : IGetStationStatusUseCase
{
    public StationStatus? Get(string stationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(stationId);

        var view = views.Find(stationId);
        if (view is null)
            return null;

        var now = clock.UtcNow;
        return new StationStatus(view, freshness.IsStale(view, now), now - view.LastMeasuredAt);
    }
}
