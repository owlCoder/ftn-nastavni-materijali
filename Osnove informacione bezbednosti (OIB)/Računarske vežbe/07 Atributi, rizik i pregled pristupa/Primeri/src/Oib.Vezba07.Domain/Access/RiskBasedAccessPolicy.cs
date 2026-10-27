using Oib.Vezba07.Domain.Access.Rules;

namespace Oib.Vezba07.Domain.Access;

public sealed class RiskBasedAccessPolicy(IEnumerable<IAccessRule>? rules, TimeSpan grantReviewInterval) : IRiskBasedAccessPolicy
{
    private readonly IReadOnlyList<IAccessRule> _rules = (rules ?? []).ToArray();

    public AccessDecision Evaluate(AccessContext? context, DateTimeOffset now)
    {
        if (context is null || string.IsNullOrWhiteSpace(context.SubjectId) ||
            string.IsNullOrWhiteSpace(context.Permission) || string.IsNullOrWhiteSpace(context.Role) ||
            string.IsNullOrWhiteSpace(context.Location) || grantReviewInterval <= TimeSpan.Zero || _rules.Count == 0 || _rules.Any(rule => rule is null))
            return new(false, AccessDecisionCodes.InvalidRequest, "Neispravni podaci ili politika pristupa.", now);

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
