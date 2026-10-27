using NUnit.Framework;
using Oib.Vezba08.Domain.Shared;
using Oib.Vezba08.Application.Analysis;
using Oib.Vezba08.Domain.Correlation;
using Oib.Vezba08.Domain.Effectiveness;
using Oib.Vezba08.Domain.Events;
using Oib.Vezba08.Infrastructure.Events;

namespace Oib.Vezba08.Tests.Application;

public sealed class AnalyzeSecurityEventsHandlerTests
{
    [Test]
    public void Analyze_CorrelatesEventsAndMeasuresTheControl()
    {
        var report = Handler(
            new SecurityEvent("corr-42", "login-failed", "ana", false),
            new SecurityEvent("corr-42", "step-up-required", "ana", true),
            new SecurityEvent("corr-42", "export-denied", "ana", true)).Analyze("step-up-export").Value!;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(report.Cases.Single().EventCount, Is.EqualTo(3));
            Assert.That(report.Effectiveness.Control, Is.EqualTo("step-up-export"));
            Assert.That(report.Effectiveness.Ratio, Is.EqualTo(2m / 3m));
            Assert.That(report.Effectiveness.MeetsTarget, Is.True);
        }
    }

    [Test]
    public void Analyze_WhenControlBlocksTooLittle_ReportsThatTargetIsMissed()
    {
        var report = Handler(
            new SecurityEvent("corr-1", "export-allowed", "ana", false),
            new SecurityEvent("corr-2", "export-allowed", "marko", false),
            new SecurityEvent("corr-3", "export-denied", "jelena", true)).Analyze("step-up-export").Value!;

        Assert.That(report.Effectiveness.MeetsTarget, Is.False);
    }

    [Test]
    public void Analyze_WhenThereAreNoEvents_HasNoCasesAndNoEvidenceOfEffectiveness()
    {
        var report = Handler().Analyze("step-up-export").Value!;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(report.Cases, Is.Empty);
            Assert.That(report.Effectiveness.MeetsTarget, Is.False);
        }
    }

    [Test]
    public void Analyze_UsesAnAlternativeEffectivenessCalculator()
    {
        var calculator = new RecordingCalculator();
        var handler = new AnalyzeSecurityEventsHandler(
            new InMemorySecurityEventSource(new[]
            {
                new SecurityEvent("corr-1", "blocked", "ana", true),
                new SecurityEvent("corr-2", "allowed", "marko", false)
            }),
            new EventCorrelator(), calculator);

        var report = handler.Analyze("export").Value!;

        Assert.That(calculator.Measurement, Is.EqualTo(new ControlMeasurement("export", 2, 1)));
        Assert.That(report.Effectiveness.Ratio, Is.EqualTo(0.25m));
    }

    private sealed class RecordingCalculator : IControlEffectivenessCalculator
    {
        public ControlMeasurement? Measurement { get; private set; }

        public Result<ControlEffectiveness> Evaluate(ControlMeasurement? measurement)
        {
            Measurement = measurement;
            return Result<ControlEffectiveness>.Ok(new ControlEffectiveness(measurement!.Control, 0.25m, false));
        }
    }

    [Test]
    public void Analyze_WhenControlIsMissing_ReturnsError()
    {
        var result = Handler().Analyze(null);
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.EqualTo("InvalidControl"));
    }

    private static AnalyzeSecurityEventsHandler Handler(params SecurityEvent[] events) =>
        new(
            new InMemorySecurityEventSource(events),
            new EventCorrelator(),
            new ControlEffectivenessCalculator(targetRatio: 0.5m));
}
