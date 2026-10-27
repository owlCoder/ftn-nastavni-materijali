using Oib.Vezba02.Application.Ports;
using Oib.Vezba02.Application.Resources.Read;
using Oib.Vezba02.Domain.Access;

namespace Oib.Vezba02.ConsoleUi;

public sealed class ResourceAccessDemo(
    IReadResourceUseCase readResource,
    IResourceAccessAuditTrail auditTrail,
    TextWriter output)
{
    public void Run()
    {
        Show("Vlasnik", DemoData.Owner, DemoData.ConfidentialRecordId);
        Show("Drugi korisnik", DemoData.OtherUser, DemoData.ConfidentialRecordId);
        Show("Revizor, poverljiv zapis", DemoData.Auditor, DemoData.ConfidentialRecordId);
        Show("Revizor, ograničen zapis", DemoData.Auditor, DemoData.RestrictedRecordId);
        Show("Nepostojeći zapis", DemoData.Owner, "rec-404");

        output.WriteLine();
        output.WriteLine("Revizijski trag:");
        foreach (var entry in auditTrail.Entries)
            output.WriteLine(
                $"{entry.ActorId} | {entry.ResourceId} | {entry.Outcome} | {entry.Code}");
    }

    private void Show(string label, Requester requester, string resourceId)
    {
        var result = readResource.Read(new ReadResourceQuery(requester, resourceId));
        output.WriteLine($"{label}: {result.Outcome} ({result.Code})");
    }
}
