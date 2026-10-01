using Oib.Vezba01.Application.Access;
using Oib.Vezba01.Application.Ports;
using Oib.Vezba01.Domain.Identity;

namespace Oib.Vezba01.ConsoleUi;

public sealed class RbacDemo(
    IAuthorizeAccessUseCase authorizeAccess,
    IAccessAuditTrail auditTrail,
    TextWriter output)
{
    public void Run()
    {
        Show("PREGLED", DemoData.Operator, DemoData.ViewReports);
        Show("IZVOZ", DemoData.Operator, DemoData.ExportReports);
        Show("BEZ PRIJAVE", DemoData.AnonymousVisitor, DemoData.ExportReports);

        output.WriteLine();
        output.WriteLine("Revizijski trag:");
        foreach (var entry in auditTrail.Entries)
            output.WriteLine(
                $"{entry.OccurredAt:O} | {entry.ActorId} | {entry.Permission} | " +
                $"{(entry.Allowed ? "ALLOW" : "DENY")} | {entry.DecisionCode}");
    }

    private void Show(string label, Actor actor, string permission)
    {
        var decision = authorizeAccess.Authorize(new AccessRequest(actor, permission));
        output.WriteLine($"{label}: {decision.Allowed} — {decision.Reason}");
    }
}
