using Oib.Vezba04.Domain.Modeling;
using Oib.Vezba04.Domain.Shared;
namespace Oib.Vezba04.Domain.Risk;
public sealed class ThreatRiskCalculator(RiskPolicy policy, IThreatScenarioValidator validator) : IThreatRiskCalculator
{
    public Result<ThreatAssessment> Assess(ThreatScenario? scenario)
    {
        var validation = validator.Validate(scenario);
        if (!validation.Success) return Result<ThreatAssessment>.Fail(validation.Error!);
        if (policy is null || policy.TrustBoundaryMultiplier < 1 || policy.TreatmentThreshold < 1)
            return Result<ThreatAssessment>.Fail(ThreatModelErrorCodes.InvalidPolicy);
        var exposure = scenario!.Flow.CrossesTrustBoundary ? policy.TrustBoundaryMultiplier : 1;
        var score = (long)scenario.Likelihood * scenario.Flow.Asset.BusinessImpact * exposure;
        if (score > int.MaxValue) return Result<ThreatAssessment>.Fail(ThreatModelErrorCodes.InvalidPolicy);
        return Result<ThreatAssessment>.Ok(new(scenario, (int)score, score >= policy.TreatmentThreshold));
    }
}
