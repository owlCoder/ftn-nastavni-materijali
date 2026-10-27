namespace Oib.Vezba07.Domain.Access.Rules;

public sealed class RiskThresholdRule(int deniedFromRiskScore) : IAccessRule
{
    public AccessDenial? Evaluate(AccessContext context) =>
        context.RiskScore >= deniedFromRiskScore
            ? new(
                AccessDecisionCodes.RiskTooHigh,
                "Rizik zahteva dodatnu proveru.",
                TimeSpan.FromHours(1))
            : null;
}
