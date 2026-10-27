using NUnit.Framework;
using Oib.Vezba04.Domain.Risk;
using Oib.Vezba04.Tests.TestData;

namespace Oib.Vezba04.Tests.Domain;

public sealed class ThreatRiskCalculatorTests
{
    private readonly ThreatRiskCalculator _calculator = new(RiskPolicy.Default, new ThreatScenarioValidator());

    [Test]
    public void Assess_WhenFlowStaysInsideTrustBoundary_MultipliesLikelihoodAndImpact()
    {
        var assessment = _calculator.Assess(Scenarios.Create(3, 5, crossesTrustBoundary: false)).Value!;

        Assert.That(assessment.RiskScore, Is.EqualTo(15));
    }

    [Test]
    public void Assess_WhenFlowCrossesTrustBoundary_DoublesTheScore()
    {
        var assessment = _calculator.Assess(Scenarios.Create(3, 5, crossesTrustBoundary: true)).Value!;

        Assert.That(assessment.RiskScore, Is.EqualTo(30));
    }

    [TestCase(3, 4, true, true)]
    [TestCase(2, 5, true, false)]
    [TestCase(5, 5, false, true)]
    public void Assess_RequiresTreatmentFromThresholdUpwards(
        int likelihood,
        int businessImpact,
        bool crossesTrustBoundary,
        bool requiresTreatment)
    {
        var assessment = _calculator.Assess(
            Scenarios.Create(likelihood, businessImpact, crossesTrustBoundary)).Value!;

        Assert.That(assessment.RequiresTreatment, Is.EqualTo(requiresTreatment));
    }

    [Test]
    public void Assess_UsesThresholdFromPolicy()
    {
        var strictCalculator = new ThreatRiskCalculator(
            RiskPolicy.Default with { TreatmentThreshold = 10 }, new ThreatScenarioValidator());

        var assessment = strictCalculator.Assess(
            Scenarios.Create(2, 5, crossesTrustBoundary: false)).Value!;

        Assert.That(assessment.RequiresTreatment, Is.True);
    }
    [Test]
    public void Assess_InvalidScenario_ReturnsError()
    {
        var result = _calculator.Assess(null);
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.EqualTo(ThreatModelErrorCodes.InvalidScenario));
    }
}
