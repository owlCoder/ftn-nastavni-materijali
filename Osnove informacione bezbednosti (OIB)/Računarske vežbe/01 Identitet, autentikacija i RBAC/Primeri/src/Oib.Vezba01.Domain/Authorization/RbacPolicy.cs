using Oib.Vezba01.Domain.Identity;

namespace Oib.Vezba01.Domain.Authorization;

public sealed class RbacPolicy : IRbacPolicy
{
    public AccessDecision Decide(
        Actor? actor,
        string? permission,
        IEnumerable<Role>? assignedRoles)
    {
        if (actor is null || string.IsNullOrWhiteSpace(actor.Id) || actor.Roles is null ||
            string.IsNullOrWhiteSpace(permission) || assignedRoles is null)
            return new(false, AccessDecisionCodes.InvalidRequest, "Neispravni podaci za autorizaciju.");

        if (!actor.IsAuthenticated)
            return new(false, AccessDecisionCodes.NotAuthenticated, "Identitet nije potvrđen.");

        var grantingRole = assignedRoles.FirstOrDefault(role => role is not null && role.Permissions is not null && Grants(role, permission));

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
