using NUnit.Framework;
using Oib.Vezba07.Domain.Access;
using Oib.Vezba07.Tests.TestData;

namespace Oib.Vezba07.Tests.Domain;

public sealed class RiskBasedAccessPolicyTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 1, 9, 0, 0, TimeSpan.Zero);

    private readonly RiskBasedAccessPolicy _policy = Contexts.Policy();

    [Test]
    public void Evaluate_WhenAllAttributesAreAcceptable_AllowsUntilNextReview()
    {
        var decision = _policy.Evaluate(Contexts.Safe, Now);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.True);
            Assert.That(decision.Code, Is.EqualTo(AccessDecisionCodes.Granted));
            Assert.That(decision.ReviewAfter, Is.EqualTo(Now.AddDays(30)));
        }
    }

    [Test]
    public void Evaluate_WhenSameUserComesFromRiskyContext_Denies()
    {
        var risky = Contexts.Safe with { ManagedDevice = false, Location = "unknown", RiskScore = 85 };

        var decision = _policy.Evaluate(risky, Now);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.False);
            Assert.That(decision.Code, Is.EqualTo(AccessDecisionCodes.UnmanagedDevice));
            Assert.That(decision.ReviewAfter, Is.EqualTo(Now.AddHours(4)));
        }
    }

    [Test]
    public void Evaluate_WhenRiskIsTooHigh_AsksForEarlyReevaluation()
    {
        var decision = _policy.Evaluate(Contexts.Safe with { RiskScore = 85 }, Now);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Code, Is.EqualTo(AccessDecisionCodes.RiskTooHigh));
            Assert.That(decision.ReviewAfter, Is.EqualTo(Now.AddHours(1)));
        }
    }

    [Test]
    public void Evaluate_WhenPolicyHasNoRules_AllowsOnlyBecauseNothingForbids()
    {
        var emptyPolicy = new RiskBasedAccessPolicy([], TimeSpan.FromDays(1));

        var decision = emptyPolicy.Evaluate(Contexts.Safe with { RiskScore = 100 }, Now);

        Assert.That(decision.Allowed, Is.True);
    }
}
