using Odp.Vezba03.Application.Contacts;
using Odp.Vezba03.Application.Ports;
using Odp.Vezba03.Domain.Contacts;
using Odp.Vezba03.Domain.Operations;
using Odp.Vezba03.Infrastructure.Stations;

namespace Odp.Vezba03.ConsoleUi;

public sealed class ContactDemo(
    IScheduleContactUseCase scheduleContact,
    InMemoryStationGateway stationGateway,
    IAuditTrail auditTrail,
    TextWriter output)
{
    public void Run()
    {
        Show(new ContactRequest("M-ARGUS", "GS-NOVI-SAD", TimeSpan.FromMinutes(10)), DemoData.Operator);
        Show(new ContactRequest("M-ARGUS", "GS-NOVI-SAD", TimeSpan.FromMinutes(40)), DemoData.Planner);
        Show(new ContactRequest("M-ARGUS", "GS-BEOGRAD", TimeSpan.FromMinutes(10)), DemoData.NightShift);

        output.WriteLine();
        output.WriteLine("Log stanice:");
        foreach (var call in stationGateway.Calls)
            output.WriteLine($"  {call.CorrelationId} | {call.StationId} | reserved={call.Reserved}");

        output.WriteLine();
        output.WriteLine("Revizijski trag:");
        foreach (var entry in auditTrail.Entries)
            output.WriteLine(
                $"  {entry.CorrelationId} | {entry.ActorId} | {entry.Action} | {entry.Target} | {entry.Outcome}");
    }

    private void Show(ContactRequest request, OperationContext context)
    {
        var result = scheduleContact.Schedule(request, context);
        output.WriteLine($"{result.CorrelationId}: {request.StationId}, {request.Duration.TotalMinutes} min -> {result.Code}");
    }
}
