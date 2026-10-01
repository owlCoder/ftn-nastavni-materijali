using NUnit.Framework;
using Oib.Vezba07.Domain.Access;
using Oib.Vezba07.Domain.Access.Rules;
using Oib.Vezba07.Tests.TestData;

namespace Oib.Vezba07.Tests.Domain;

public sealed class AccessRuleTests
{
    [Test]
    public void BusinessRoleRule_WhenRoleIsNotResponsible_Denies()
    {
        var denial = new BusinessRoleRule(["Operator"])
            .Evaluate(Contexts.Safe with { Role = "Guest" });

        Assert.That(denial?.Code, Is.EqualTo(AccessDecisionCodes.RoleNotResponsible));
    }

    [Test]
    public void ManagedDeviceRule_WhenRestrictedResourceIsOpenedFromUnmanagedDevice_Denies()
    {
        var denial = new ManagedDeviceRule()
            .Evaluate(Contexts.Safe with { ManagedDevice = false });

        Assert.That(denial?.Code, Is.EqualTo(AccessDecisionCodes.UnmanagedDevice));
    }

    [Test]
    public void ManagedDeviceRule_WhenResourceIsNotRestricted_DoesNotApply()
    {
        var denial = new ManagedDeviceRule()
            .Evaluate(Contexts.Safe with { ManagedDevice = false, RestrictedResource = false });

        Assert.That(denial, Is.Null);
    }

    [Test]
    public void TrustedLocationRule_WhenRestrictedResourceIsOpenedFromUnknownLocation_Denies()
    {
        var denial = new TrustedLocationRule(["office", "vpn"])
            .Evaluate(Contexts.Safe with { Location = "unknown" });

        Assert.That(denial?.Code, Is.EqualTo(AccessDecisionCodes.UntrustedLocation));
    }

    [TestCase(69, false)]
    [TestCase(70, true)]
    public void RiskThresholdRule_DeniesFromConfiguredRiskScore(int riskScore, bool denied)
    {
        var denial = new RiskThresholdRule(deniedFromRiskScore: 70)
            .Evaluate(Contexts.Safe with { RiskScore = riskScore });

        Assert.That(denial is not null, Is.EqualTo(denied));
    }
}
