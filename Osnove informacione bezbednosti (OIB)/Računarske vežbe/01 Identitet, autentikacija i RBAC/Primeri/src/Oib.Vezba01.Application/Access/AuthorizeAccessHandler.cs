using Oib.Vezba01.Application.Ports;
using Oib.Vezba01.Domain.Authorization;

namespace Oib.Vezba01.Application.Access;

public sealed class AuthorizeAccessHandler(
    IRoleCatalog roleCatalog,
    RbacPolicy policy,
    IAccessAuditLog auditLog,
    IClock clock) : IAuthorizeAccessUseCase
{
    public AccessDecision Authorize(AccessRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var assignedRoles = request.Actor.Roles
            .Select(roleCatalog.FindByName)
            .OfType<Role>()
            .ToArray();

        var decision = policy.Decide(request.Actor, request.Permission, assignedRoles);

        auditLog.Record(new AccessAuditEntry(
            clock.UtcNow,
            request.Actor.Id,
            request.Permission,
            decision.Allowed,
            decision.Code));

        return decision;
    }
}
