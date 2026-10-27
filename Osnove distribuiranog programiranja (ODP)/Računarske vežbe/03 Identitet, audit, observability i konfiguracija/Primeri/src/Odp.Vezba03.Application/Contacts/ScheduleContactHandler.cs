using Odp.Vezba03.Application.Audit;
using Odp.Vezba03.Application.Ports;
using Odp.Vezba03.Domain.Contacts;
using Odp.Vezba03.Domain.Operations;

namespace Odp.Vezba03.Application.Contacts;

public sealed class ScheduleContactHandler(
    ContactRequestValidator validator,
    IStationGateway stationGateway,
    IAuditLog auditLog,
    IClock clock) : IScheduleContactUseCase
{
    private const string Action = "contact.schedule";

    public ScheduleContactResult Schedule(ContactRequest request, OperationContext context)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(context);

        var code = Decide(request, context);

        auditLog.Record(new AuditEntry(
            clock.UtcNow,
            context.CorrelationId,
            context.ActorId,
            Action,
            request.StationId,
            code));

        return new ScheduleContactResult(code == ContactCodes.Scheduled, code, context.CorrelationId);
    }

    private string Decide(ContactRequest request, OperationContext context)
    {
        var validation = validator.Validate(request);
        if (!validation.Success)
            return validation.Error!;

        return stationGateway.Reserve(request, context.CorrelationId)
            ? ContactCodes.Scheduled
            : ContactCodes.StationUnavailable;
    }
}
