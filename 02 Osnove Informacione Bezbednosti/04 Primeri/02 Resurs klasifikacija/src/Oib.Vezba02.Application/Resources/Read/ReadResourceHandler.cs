using Oib.Vezba02.Application.Ports;
using Oib.Vezba02.Domain.Access;

namespace Oib.Vezba02.Application.Resources.Read;

public sealed class ReadResourceHandler(
    IProtectedResourceRepository resources,
    ResourceReadPolicy policy,
    IResourceAccessAuditLog auditLog) : IReadResourceUseCase
{
    public ReadResourceResult Read(ReadResourceQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        var result = Decide(query);

        auditLog.Record(new ResourceAccessAuditEntry(
            query.Requester.ActorId,
            query.ResourceId,
            result.Outcome,
            result.Code));

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
