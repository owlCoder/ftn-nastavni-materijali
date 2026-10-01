using Oib.Vezba04.Domain.Modeling;

namespace Oib.Vezba04.Application.Ports;

public interface IThreatScenarioRepository
{
    IReadOnlyList<ThreatScenario> GetAll();
}
