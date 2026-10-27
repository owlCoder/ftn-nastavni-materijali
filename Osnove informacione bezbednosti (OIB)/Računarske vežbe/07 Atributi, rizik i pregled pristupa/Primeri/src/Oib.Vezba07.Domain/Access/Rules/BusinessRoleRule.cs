namespace Oib.Vezba07.Domain.Access.Rules;

public sealed class BusinessRoleRule(IEnumerable<string> responsibleRoles) : IAccessRule
{
    private readonly IReadOnlySet<string> _responsibleRoles =
        responsibleRoles.ToHashSet(StringComparer.OrdinalIgnoreCase);

    public AccessDenial? Evaluate(AccessContext context) =>
        _responsibleRoles.Contains(context.Role)
            ? null
            : new(
                AccessDecisionCodes.RoleNotResponsible,
                "Uloga nema poslovnu odgovornost.",
                TimeSpan.FromDays(1));
}
