using Oib.Vezba06.Domain.Shared;
using Oib.Vezba06.Application.Ports;
using Oib.Vezba06.Domain.Incidents;

namespace Oib.Vezba06.Infrastructure.Incidents;

public sealed class InMemoryIncidentRepository : IIncidentRepository
{
    private readonly List<Incident> _incidents = [];

    public IReadOnlyList<Incident> Incidents => _incidents;

    public Result<bool> Add(Incident? incident)
    {
        if (incident is null) return Result<bool>.Fail("InvalidIncident");
        _incidents.Add(incident);
        return Result<bool>.Ok(true);
    }
}
