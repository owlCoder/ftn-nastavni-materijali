using Oib.Vezba04.Domain.Modeling;
using Oib.Vezba04.Domain.Shared;

namespace Oib.Vezba04.Domain.Risk;

public sealed class ThreatScenarioValidator
{
    public Result Validate(ThreatScenario scenario)
    {
        ArgumentNullException.ThrowIfNull(scenario);

        if (!RiskScale.Contains(scenario.Likelihood))
            return Result.Fail(ThreatModelErrorCodes.LikelihoodOutOfRange);
        if (!RiskScale.Contains(scenario.Flow.Asset.BusinessImpact))
            return Result.Fail(ThreatModelErrorCodes.BusinessImpactOutOfRange);

        return Result.Ok();
    }
}
