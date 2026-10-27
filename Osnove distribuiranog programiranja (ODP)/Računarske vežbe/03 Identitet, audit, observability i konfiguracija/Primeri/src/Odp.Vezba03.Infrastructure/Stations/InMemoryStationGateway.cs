using Odp.Vezba03.Application.Ports;
using Odp.Vezba03.Domain.Contacts;

namespace Odp.Vezba03.Infrastructure.Stations;

/// <summary>Zamena za udaljenu stanicu; beleži pozive kao što bi to radio njen log.</summary>
public sealed class InMemoryStationGateway(IEnumerable<string> availableStations) : IStationGateway
{
    private readonly HashSet<string> _available = new(availableStations, StringComparer.Ordinal);
    private readonly List<StationCall> _calls = [];

    public IReadOnlyList<StationCall> Calls => _calls;

    public bool Reserve(ContactRequest request, string correlationId)
    {
        var reserved = _available.Contains(request.StationId);
        _calls.Add(new StationCall(request.StationId, correlationId, reserved));
        return reserved;
    }
}
