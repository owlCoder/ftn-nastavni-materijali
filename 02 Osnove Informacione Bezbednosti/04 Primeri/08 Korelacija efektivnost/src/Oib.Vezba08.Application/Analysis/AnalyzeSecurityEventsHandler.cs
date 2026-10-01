using Oib.Vezba08.Application.Ports;
using Oib.Vezba08.Domain.Correlation;
using Oib.Vezba08.Domain.Effectiveness;

namespace Oib.Vezba08.Application.Analysis;

public sealed class AnalyzeSecurityEventsHandler(
    ISecurityEventSource eventSource,
    EventCorrelator correlator,
    ControlEffectivenessCalculator effectivenessCalculator) : IAnalyzeSecurityEventsUseCase
{
    public SecurityAnalysisReport Analyze(string control)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(control);

        var events = eventSource.GetEvents();
        var measurement = new ControlMeasurement(
            control,
            events.Count,
            events.Count(securityEvent => securityEvent.Blocked));

        return new SecurityAnalysisReport(
            correlator.Correlate(events),
            effectivenessCalculator.Evaluate(measurement));
    }
}
