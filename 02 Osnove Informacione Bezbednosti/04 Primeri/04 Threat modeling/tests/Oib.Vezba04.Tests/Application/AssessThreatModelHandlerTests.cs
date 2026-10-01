using NUnit.Framework;
using Oib.Vezba04.Application.Assessment;
using Oib.Vezba04.Domain.Modeling;
using Oib.Vezba04.Domain.Risk;
using Oib.Vezba04.Infrastructure.Modeling;
using Oib.Vezba04.Tests.TestData;

namespace Oib.Vezba04.Tests.Application;

public sealed class AssessThreatModelHandlerTests
{
    [Test]
    public void Assess_OrdersScenariosFromHighestToLowestRisk()
    {
        var report = Handler(
            Scenarios.Create(2, 2, false, "nizak"),
            Scenarios.Create(3, 5, true, "visok"),
            Scenarios.Create(3, 3, false, "srednji")).Assess();

        Assert.That(
            report.Assessments.Select(assessment => assessment.Scenario.Name),
            Is.EqualTo(new[] { "visok", "srednji", "nizak" }));
    }

    [Test]
    public void Assess_WhenHighImpactFlowCrossesTrustBoundary_RequiresTreatment()
    {
        var report = Handler(Scenarios.Create(3, 5, true)).Assess();

        Assert.That(report.Assessments.Single().RequiresTreatment, Is.True);
    }

    [Test]
    public void Assess_WhenScenarioIsNotValid_RejectsItInsteadOfScoringIt()
    {
        var report = Handler(
            Scenarios.Create(0, 5, true, "bez verovatnoće"),
            Scenarios.Create(3, 3, false, "ispravan")).Assess();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(
                report.Rejected,
                Is.EqualTo(new[]
                {
                    new RejectedScenario(
                        "bez verovatnoće",
                        ThreatModelErrorCodes.LikelihoodOutOfRange)
                }));
            Assert.That(report.Assessments, Has.Count.EqualTo(1));
        }
    }

    private static AssessThreatModelHandler Handler(params ThreatScenario[] scenarios) =>
        new(
            new InMemoryThreatScenarioRepository(scenarios),
            new ThreatScenarioValidator(),
            new ThreatRiskCalculator(RiskPolicy.Default));
}
