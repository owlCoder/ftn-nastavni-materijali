using Oib.Vezba02.Application.Ports;
using Oib.Vezba02.Domain.Access;

namespace Oib.Vezba02.Application.Resources.Read;

public sealed class ReadResourceHandler(
    IProtectedResourceRepository resources,
    IResourceReadPolicy policy,
    IResourceAccessAuditLog auditLog) : IReadResourceUseCase
{
    public ReadResourceResult Read(ReadResourceQuery? query)
    {
        if (query?.Requester is null || string.IsNullOrWhiteSpace(query.Requester.ActorId) ||
            query.Requester.Permissions is null || string.IsNullOrWhiteSpace(query.ResourceId))
            return new(ReadResourceOutcome.Denied, ResourceAccessCodes.InvalidRequest, null);

        var result = Decide(query);

        var auditResult = auditLog.Record(new ResourceAccessAuditEntry(
            query.Requester.ActorId,
            query.ResourceId,
            result.Outcome,
            result.Code));

        if (!auditResult.Success)
            return new(ReadResourceOutcome.Denied, ResourceAccessCodes.AuditFailed, null);

        return result;
    }

    private ReadResourceResult Decide(ReadResourceQuery query)
    {
        var resource = resources.FindById(query.ResourceId);
        if (resource is null)
            return new(ReadResourceOutcome.NotFound, ResourceAccessCodes.ResourceNotFound, null);

        var decision = policy.Decide(query.Requester, resource);

        return decision.Allowed
            ? new(ReadResourceOutcome.Granted, decision.Code, resource)
            : new(ReadResourceOutcome.Denied, decision.Code, null);
    }
}
