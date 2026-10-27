using Oib.Vezba01.Application.Ports;
using Oib.Vezba01.Domain.Authorization;

namespace Oib.Vezba01.Application.Access;

public sealed class AuthorizeAccessHandler(
    IRoleCatalog roleCatalog,
    IRbacPolicy policy,
    IAccessAuditLog auditLog,
    IClock clock) : IAuthorizeAccessUseCase
{
    public AccessDecision Authorize(AccessRequest? request)
    {
        if (request?.Actor is null || string.IsNullOrWhiteSpace(request.Actor.Id) ||
            request.Actor.Roles is null || request.Actor.Roles.Any(string.IsNullOrWhiteSpace) || string.IsNullOrWhiteSpace(request.Permission))
            return new(false, AccessDecisionCodes.InvalidRequest, "Neispravan zahtev za pristup.");

        var assignedRoles = request.Actor.Roles
            .Select(roleCatalog.FindByName)
            .OfType<Role>()
            .ToArray();

        var decision = policy.Decide(request.Actor, request.Permission, assignedRoles);

        var auditResult = auditLog.Record(new AccessAuditEntry(
            clock.UtcNow,
            request.Actor.Id,
            request.Permission,
            decision.Allowed,
            decision.Code));

        if (!auditResult.Success)
            return new(false, AccessDecisionCodes.AuditFailed, "Nije moguće evidentirati odluku.");

        return decision;
    }
}
