using NUnit.Framework;
using Oib.Vezba04.Domain.Risk;
using Oib.Vezba04.Tests.TestData;

namespace Oib.Vezba04.Tests.Domain;

public sealed class ThreatScenarioValidatorTests
{
    private readonly ThreatScenarioValidator _validator = new();

    [TestCase(1, 1)]
    [TestCase(5, 5)]
    public void Validate_WhenValuesAreOnTheScale_Succeeds(int likelihood, int businessImpact)
    {
        var result = _validator.Validate(Scenarios.Create(likelihood, businessImpact, false));

        Assert.That(result.Success, Is.True);
    }

    [TestCase(0)]
    [TestCase(6)]
    public void Validate_WhenLikelihoodIsOffTheScale_ReturnsStableCode(int likelihood)
    {
        var result = _validator.Validate(Scenarios.Create(likelihood, 3, false));

        Assert.That(result.Error, Is.EqualTo(ThreatModelErrorCodes.LikelihoodOutOfRange));
    }

    [TestCase(0)]
    [TestCase(6)]
    public void Validate_WhenBusinessImpactIsOffTheScale_ReturnsStableCode(int businessImpact)
    {
        var result = _validator.Validate(Scenarios.Create(3, businessImpact, false));

        Assert.That(result.Error, Is.EqualTo(ThreatModelErrorCodes.BusinessImpactOutOfRange));
    }
}
