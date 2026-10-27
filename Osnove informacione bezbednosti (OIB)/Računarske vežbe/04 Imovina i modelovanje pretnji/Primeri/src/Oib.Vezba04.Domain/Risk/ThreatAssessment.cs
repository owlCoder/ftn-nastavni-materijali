using Oib.Vezba04.Domain.Modeling;

namespace Oib.Vezba04.Domain.Risk;

public sealed record ThreatAssessment(
    ThreatScenario Scenario,
    int RiskScore,
    bool RequiresTreatment);
