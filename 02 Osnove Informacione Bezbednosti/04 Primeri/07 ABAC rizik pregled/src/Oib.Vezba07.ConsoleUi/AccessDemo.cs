using Oib.Vezba07.Application.Access;
using Oib.Vezba07.Application.Ports;
using Oib.Vezba07.Domain.Access;

namespace Oib.Vezba07.ConsoleUi;

public sealed class AccessDemo(
    IEvaluateAccessUseCase evaluateAccess,
    IScheduledAccessReviews scheduledReviews,
    TextWriter output)
{
    public void Run()
    {
        var baseline = DemoData.Baseline;

        Show("Upravljani uređaj", baseline);
        Show(
            "Nepoznat uređaj",
            baseline with { ManagedDevice = false, Location = "unknown", RiskScore = 85 });
        Show("Nepoznata lokacija", baseline with { Location = "unknown" });
        Show("Povišen rizik", baseline with { RiskScore = 85 });
        Show("Druga uloga", baseline with { Role = "Guest" });

        output.WriteLine();
        output.WriteLine("Zakazani pregledi pristupa:");
        foreach (var item in scheduledReviews.Items)
            output.WriteLine(
                $"{item.SubjectId} | {item.Permission} | vlasnik={item.BusinessOwner} | " +
                $"rok={item.ReviewDue:yyyy-MM-dd}");
    }

    private void Show(string label, AccessContext context)
    {
        var decision = evaluateAccess.Evaluate(
            new EvaluateAccessRequest(context, DemoData.BusinessOwner));

        output.WriteLine($"{label}: {decision.Allowed} — {decision.Reason}");
    }
}
