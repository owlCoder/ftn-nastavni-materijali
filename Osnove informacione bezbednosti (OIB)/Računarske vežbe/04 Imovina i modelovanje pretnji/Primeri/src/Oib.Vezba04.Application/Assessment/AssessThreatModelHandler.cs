using Oib.Vezba04.Application.Ports;
using Oib.Vezba04.Domain.Risk;

namespace Oib.Vezba04.Application.Assessment;

public sealed class AssessThreatModelHandler(
    IThreatScenarioRepository scenarios,
    IThreatScenarioValidator validator,
    IThreatRiskCalculator calculator) : IAssessThreatModelUseCase
{
    public ThreatModelReport Assess()
    {
        var assessments = new List<ThreatAssessment>();
        var rejected = new List<RejectedScenario>();

        foreach (var scenario in scenarios.GetAll())
        {
            var validation = validator.Validate(scenario);
            if (!validation.Success)
            {
                rejected.Add(new RejectedScenario(scenario?.Name ?? "(nepoznat scenario)", validation.Error));
                continue;
            }
            var result = calculator.Assess(scenario);
            if (result.Success && result.Value is not null)
                assessments.Add(result.Value);
            else
                rejected.Add(new RejectedScenario(scenario!.Name, result.Error ?? "AssessmentFailed"));
        }

        return new ThreatModelReport(
            assessments.OrderByDescending(assessment => assessment.RiskScore).ToArray(),
            rejected);
    }
}
