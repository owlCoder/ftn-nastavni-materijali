namespace Oib.Vezba07.Domain.Access.Rules;

public sealed class TrustedLocationRule(IEnumerable<string> trustedLocations) : IAccessRule
{
    private readonly IReadOnlySet<string> _trustedLocations =
        trustedLocations.ToHashSet(StringComparer.OrdinalIgnoreCase);

    public AccessDenial? Evaluate(AccessContext context) =>
        context.RestrictedResource && !_trustedLocations.Contains(context.Location)
            ? new(
                AccessDecisionCodes.UntrustedLocation,
                "Ograničen resurs zahteva lokaciju od poverenja.",
                TimeSpan.FromHours(4))
            : null;
}
