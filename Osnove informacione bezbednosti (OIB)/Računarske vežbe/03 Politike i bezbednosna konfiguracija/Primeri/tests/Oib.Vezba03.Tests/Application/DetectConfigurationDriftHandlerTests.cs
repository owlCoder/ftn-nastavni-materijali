using NUnit.Framework;
using Oib.Vezba03.Application.Ports;
using Oib.Vezba03.Domain.Shared;
using Oib.Vezba03.Application.Drift;
using Oib.Vezba03.Domain.Configuration;
using Oib.Vezba03.Domain.Drift;
using Oib.Vezba03.Domain.Drift.Controls;
using Oib.Vezba03.Infrastructure.Configuration;
using Oib.Vezba03.Tests.TestDoubles;

namespace Oib.Vezba03.Tests.Application;

public sealed class DetectConfigurationDriftHandlerTests
{
    private static readonly SecurityBaseline Baseline = new("2026.1", 14, true);

    private RecordingDriftReportLog _reportLog = null!;

    [SetUp]
    public void SetUp() => _reportLog = new RecordingDriftReportLog();

    [Test]
    public void Detect_WhenConfigurationIsWeakerThanBaseline_ReportsEveryDrift()
    {
        var report = Handler(new SecurityConfiguration(10, false)).Detect().Value!;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(report.IsCompliant, Is.False);
            Assert.That(
                report.Findings.Select(finding => finding.Control),
                Is.EqualTo(new[] { MinimumPasswordLengthControl.Name, AdminMfaControl.Name }));
        }
    }

    [Test]
    public void Detect_WhenConfigurationMatchesBaseline_IsCompliant()
    {
        var report = Handler(new SecurityConfiguration(14, true)).Detect().Value!;

        Assert.That(report.IsCompliant, Is.True);
    }

    [Test]
    public void Detect_TagsReportWithCorrelationIdAndBaselineVersion()
    {
        var report = Handler(new SecurityConfiguration(10, true)).Detect().Value!;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(report.CorrelationId, Is.EqualTo("corr-1"));
            Assert.That(report.BaselineVersion, Is.EqualTo("2026.1"));
        }
    }

    [Test]
    public void Detect_MakesTheReportVisibleThroughTheLog()
    {
        var report = Handler(new SecurityConfiguration(10, false)).Detect().Value!;

        Assert.That(_reportLog.Reports, Is.EqualTo(new[] { report }));
    }

    private DetectConfigurationDriftHandler Handler(SecurityConfiguration current) =>
        new(
            new InMemorySecurityBaselineProvider(Baseline),
            new InMemoryActiveConfigurationProvider(current),
            new ConfigurationDriftDetector(
            [
                new MinimumPasswordLengthControl(),
                new AdminMfaControl()
            ]),
            new FixedCorrelationIdGenerator("corr-1"),
            _reportLog);
    [Test]
    public void Detect_InvalidConfiguration_ReturnsErrorInsteadOfCompliantReport()
    {
        var result = Handler(new SecurityConfiguration(0, true)).Detect();
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.EqualTo("InvalidConfiguration"));
        Assert.That(_reportLog.Reports, Is.Empty);
    }
    [Test]
    public void Detect_WhenReportCannotBeRecorded_ReturnsError()
    {
        var handler = new DetectConfigurationDriftHandler(
            new InMemorySecurityBaselineProvider(Baseline),
            new InMemoryActiveConfigurationProvider(new SecurityConfiguration(14, true)),
            new ConfigurationDriftDetector([new MinimumPasswordLengthControl()]),
            new FixedCorrelationIdGenerator("corr-1"), new FailingLog());
        var result = handler.Detect();
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.EqualTo("ReportWriteFailed"));
    }

    private sealed class FailingLog : IDriftReportLog
    {
        public Result<bool> Record(DriftReport? report) => Result<bool>.Fail("ReportWriteFailed");
    }
}
