namespace Oib.Vezba01;

public sealed class RbacAuthorizer(RoleCatalog catalog)
{
    public AccessDecision Authorize(Actor actor, string permission)
    {
        if (!actor.IsAuthenticated) return new(false, "Identitet nije potvrđen.");

        var grantingRole = actor.Roles.FirstOrDefault(role => catalog.Grants(role, permission));
        return grantingRole is null
            ? new(false, $"Nijedna uloga ne dodeljuje dozvolu '{permission}'.")
            : new(true, $"Uloga '{grantingRole}' dodeljuje dozvolu '{permission}'.");
    }
}

