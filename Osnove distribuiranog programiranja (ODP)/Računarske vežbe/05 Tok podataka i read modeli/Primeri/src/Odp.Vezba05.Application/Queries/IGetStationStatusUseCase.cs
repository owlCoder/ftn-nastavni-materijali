namespace Odp.Vezba05.Application.Queries;

public interface IGetStationStatusUseCase
{
    StationStatus? Get(string stationId);
}
