using Odp.Vezba05.Domain.ReadModels;

namespace Odp.Vezba05.Application.Ports;

public interface IStationViewStore
{
    StationView? Find(string stationId);

    void Save(StationView view);
}
