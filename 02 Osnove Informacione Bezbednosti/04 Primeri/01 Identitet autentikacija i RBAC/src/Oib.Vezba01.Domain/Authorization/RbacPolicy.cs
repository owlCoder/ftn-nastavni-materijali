using Oib.Vezba01.Domain.Identity;

namespace Oib.Vezba01.Domain.Authorization;

public sealed class RbacPolicy
{
    public AccessDecision Decide(
        Actor actor,
        string permission,
        IEnumerable<Role> assignedRoles)
    {
        ArgumentNullException.ThrowIfNull(actor);
        ArgumentException.ThrowIfNullOrWhiteSpace(permission);
        ArgumentNullException.ThrowIfNull(assignedRoles);

        if (!actor.IsAuthenticated)
            return new(false, AccessDecisionCodes.NotAuthenticated, "Identitet nije potvrđen.");

        var grantingRole = assignedRoles.FirstOrDefault(role => Grants(role, permission));

        return grantingRole is null
            ? new(
                false,
                AccessDecisionCodes.PermissionNotGranted,
                $"Nijedna uloga ne dodeljuje dozvolu '{permission}'.")
            : new(
                true,
                AccessDecisionCodes.GrantedByRole,
                $"Uloga '{grantingRole.Name}' dodeljuje dozvolu '{permission}'.");
    }

    private static bool Grants(Role role, string permission) =>
        role.Permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
}
