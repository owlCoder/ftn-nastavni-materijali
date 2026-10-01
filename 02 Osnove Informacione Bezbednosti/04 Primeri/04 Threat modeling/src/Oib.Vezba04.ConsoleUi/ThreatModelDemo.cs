using Oib.Vezba04.Application.Assessment;

namespace Oib.Vezba04.ConsoleUi;

public sealed class ThreatModelDemo(IAssessThreatModelUseCase assessThreatModel, TextWriter output)
{
    public void Run()
    {
        var report = assessThreatModel.Assess();

        output.WriteLine("Scenariji po prioritetu:");
        foreach (var assessment in report.Assessments)
        {
            output.WriteLine(
                $"rizik={assessment.RiskScore,2} | " +
                $"{(assessment.RequiresTreatment ? "TRETMAN" : "prihvatljivo")} | " +
                $"{assessment.Scenario.Name}");
            output.WriteLine($"          kontrola: {assessment.Scenario.ProposedControl}");
        }

        output.WriteLine();
        output.WriteLine("Scenariji vraćeni na doradu:");
        foreach (var rejected in report.Rejected)
            output.WriteLine($"{rejected.ErrorCode} | {rejected.ScenarioName}");
    }
}
