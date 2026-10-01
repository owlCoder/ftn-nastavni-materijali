using Oib.Vezba04.Domain.Modeling;

namespace Oib.Vezba04.Domain.Risk;

public sealed class ThreatRiskCalculator(RiskPolicy policy)
{
    public ThreatAssessment Assess(ThreatScenario scenario)
    {
        ArgumentNullException.ThrowIfNull(scenario);

        var score = Score(scenario);
        return new ThreatAssessment(scenario, score, score >= policy.TreatmentThreshold);
    }

    private int Score(ThreatScenario scenario)
    {
        var exposure = scenario.Flow.CrossesTrustBoundary ? policy.TrustBoundaryMultiplier : 1;
        return scenario.Likelihood * scenario.Flow.Asset.BusinessImpact * exposure;
    }
}
