using Oib.Vezba06.Application.Ports;
using Oib.Vezba06.Domain.Incidents;

namespace Oib.Vezba06.Infrastructure.Incidents;

public sealed class InMemoryIncidentRepository : IIncidentRepository
{
    private readonly List<Incident> _incidents = [];

    public IReadOnlyList<Incident> Incidents => _incidents;

    public void Add(Incident incident)
    {
        ArgumentNullException.ThrowIfNull(incident);
        _incidents.Add(incident);
    }
}
