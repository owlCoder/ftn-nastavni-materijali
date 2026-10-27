using Oib.Vezba04.Domain.Modeling;
using Oib.Vezba04.Domain.Shared;
namespace Oib.Vezba04.Domain.Risk;
public interface IThreatRiskCalculator
{
    Result<ThreatAssessment> Assess(ThreatScenario? scenario);
}
