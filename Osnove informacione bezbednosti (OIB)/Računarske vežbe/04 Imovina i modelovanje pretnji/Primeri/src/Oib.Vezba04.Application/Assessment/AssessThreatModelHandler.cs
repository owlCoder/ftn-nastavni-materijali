using Oib.Vezba04.Application.Ports;
using Oib.Vezba04.Domain.Risk;

namespace Oib.Vezba04.Application.Assessment;

public sealed class AssessThreatModelHandler(
    IThreatScenarioRepository scenarios,
    ThreatScenarioValidator validator,
    ThreatRiskCalculator calculator) : IAssessThreatModelUseCase
{
    public ThreatModelReport Assess()
    {
        var assessments = new List<ThreatAssessment>();
        var rejected = new List<RejectedScenario>();

        foreach (var scenario in scenarios.GetAll())
        {
            var validation = validator.Validate(scenario);
            if (validation.Success)
                assessments.Add(calculator.Assess(scenario));
            else
                rejected.Add(new RejectedScenario(scenario.Name, validation.Error));
        }

        return new ThreatModelReport(
            assessments.OrderByDescending(assessment => assessment.RiskScore).ToArray(),
            rejected);
    }
}
