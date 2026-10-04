using Odp.Vezba05.Application.Ports;
using Odp.Vezba05.Domain.ReadModels;

namespace Odp.Vezba05.Infrastructure.ReadModels;

public sealed class InMemoryStationViewStore : IStationViewStore
{
    private readonly Dictionary<string, StationView> _views = new(StringComparer.Ordinal);

    public StationView? Find(string stationId) => _views.GetValueOrDefault(stationId);

    public void Save(StationView view) => _views[view.StationId] = view;
}
