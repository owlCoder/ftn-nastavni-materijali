using Oib.Vezba07.Domain.Access.Rules;

namespace Oib.Vezba07.Domain.Access;

public sealed class RiskBasedAccessPolicy(IEnumerable<IAccessRule> rules, TimeSpan grantReviewInterval)
{
    private readonly IReadOnlyList<IAccessRule> _rules = rules.ToArray();

    public AccessDecision Evaluate(AccessContext context, DateTimeOffset now)
    {
        ArgumentNullException.ThrowIfNull(context);

        var denial = _rules
            .Select(rule => rule.Evaluate(context))
            .FirstOrDefault(outcome => outcome is not null);

        return denial is null
            ? new(
                true,
                AccessDecisionCodes.Granted,
                "Atributi i rizik su prihvatljivi.",
                now + grantReviewInterval)
            : new(false, denial.Code, denial.Reason, now + denial.ReviewIn);
    }
}
