namespace Oib.Vezba04;

public sealed class ThreatModelService
{
    public int RiskScore(ThreatScenario scenario) =>
        scenario.Likelihood * scenario.Flow.Asset.BusinessImpact * (scenario.Flow.CrossesTrustBoundary ? 2 : 1);

    public bool RequiresTreatment(ThreatScenario scenario) => RiskScore(scenario) >= 24;
}

