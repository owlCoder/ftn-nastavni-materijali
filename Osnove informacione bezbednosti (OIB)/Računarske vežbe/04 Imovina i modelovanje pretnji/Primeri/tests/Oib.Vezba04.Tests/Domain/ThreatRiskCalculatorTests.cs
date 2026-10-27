using NUnit.Framework;
using Oib.Vezba04.Domain.Risk;
using Oib.Vezba04.Tests.TestData;

namespace Oib.Vezba04.Tests.Domain;

public sealed class ThreatRiskCalculatorTests
{
    private readonly ThreatRiskCalculator _calculator = new(RiskPolicy.Default);

    [Test]
    public void Assess_WhenFlowStaysInsideTrustBoundary_MultipliesLikelihoodAndImpact()
    {
        var assessment = _calculator.Assess(Scenarios.Create(3, 5, crossesTrustBoundary: false));

        Assert.That(assessment.RiskScore, Is.EqualTo(15));
    }

    [Test]
    public void Assess_WhenFlowCrossesTrustBoundary_DoublesTheScore()
    {
        var assessment = _calculator.Assess(Scenarios.Create(3, 5, crossesTrustBoundary: true));

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
            Scenarios.Create(likelihood, businessImpact, crossesTrustBoundary));

        Assert.That(assessment.RequiresTreatment, Is.EqualTo(requiresTreatment));
    }

    [Test]
    public void Assess_UsesThresholdFromPolicy()
    {
        var strictCalculator = new ThreatRiskCalculator(
            RiskPolicy.Default with { TreatmentThreshold = 10 });

        var assessment = strictCalculator.Assess(
            Scenarios.Create(2, 5, crossesTrustBoundary: false));

        Assert.That(assessment.RequiresTreatment, Is.True);
    }
}
