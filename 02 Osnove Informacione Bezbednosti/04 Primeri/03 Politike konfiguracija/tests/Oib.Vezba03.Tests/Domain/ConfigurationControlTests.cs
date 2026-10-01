using NUnit.Framework;
using Oib.Vezba03.Domain.Configuration;
using Oib.Vezba03.Domain.Drift;
using Oib.Vezba03.Domain.Drift.Controls;

namespace Oib.Vezba03.Tests.Domain;

public sealed class ConfigurationControlTests
{
    private static readonly SecurityBaseline Baseline = new("2026.1", 14, true);

    [Test]
    public void MinimumPasswordLength_WhenShorterThanBaseline_ReportsFinding()
    {
        var finding = new MinimumPasswordLengthControl().Check(
            Baseline,
            new SecurityConfiguration(10, true));

        Assert.That(
            finding,
            Is.EqualTo(new ConfigurationFinding(MinimumPasswordLengthControl.Name, "14", "10")));
    }

    [TestCase(14)]
    [TestCase(20)]
    public void MinimumPasswordLength_WhenAtLeastBaseline_ReportsNothing(int length)
    {
        var finding = new MinimumPasswordLengthControl().Check(
            Baseline,
            new SecurityConfiguration(length, true));

        Assert.That(finding, Is.Null);
    }

    [Test]
    public void AdminMfa_WhenBaselineRequiresItAndConfigurationDoesNot_ReportsFinding()
    {
        var finding = new AdminMfaControl().Check(Baseline, new SecurityConfiguration(14, false));

        Assert.That(
            finding,
            Is.EqualTo(new ConfigurationFinding(AdminMfaControl.Name, "True", "False")));
    }

    [Test]
    public void AdminMfa_WhenConfigurationIsStricterThanBaseline_ReportsNothing()
    {
        var relaxedBaseline = Baseline with { RequireMfaForAdmins = false };

        var finding = new AdminMfaControl().Check(
            relaxedBaseline,
            new SecurityConfiguration(14, true));

        Assert.That(finding, Is.Null);
    }
}
