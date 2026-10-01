using Oib.Vezba04.Application.Ports;
using Oib.Vezba04.Domain.Modeling;

namespace Oib.Vezba04.Infrastructure.Modeling;

public sealed class InMemoryThreatScenarioRepository(IEnumerable<ThreatScenario> scenarios)
    : IThreatScenarioRepository
{
    private readonly IReadOnlyList<ThreatScenario> _scenarios = scenarios.ToArray();

    public IReadOnlyList<ThreatScenario> GetAll() => _scenarios;
}
