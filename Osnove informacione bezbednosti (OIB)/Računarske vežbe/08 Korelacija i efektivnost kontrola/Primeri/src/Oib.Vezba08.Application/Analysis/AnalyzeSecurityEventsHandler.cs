using Oib.Vezba08.Application.Ports;
using Oib.Vezba08.Domain.Correlation;
using Oib.Vezba08.Domain.Effectiveness;
using Oib.Vezba08.Domain.Shared;
namespace Oib.Vezba08.Application.Analysis;
public sealed class AnalyzeSecurityEventsHandler(
    ISecurityEventSource eventSource, IEventCorrelator correlator,
    IControlEffectivenessCalculator effectivenessCalculator) : IAnalyzeSecurityEventsUseCase
{
    public Result<SecurityAnalysisReport> Analyze(string? control)
    {
        if (string.IsNullOrWhiteSpace(control))
            return Result<SecurityAnalysisReport>.Fail("InvalidControl");
        var events = eventSource.GetEvents();
        var cases = correlator.Correlate(events);
        if (!cases.Success || cases.Value is null)
            return Result<SecurityAnalysisReport>.Fail(cases.Error ?? "CorrelationFailed");
        var measurement = new ControlMeasurement(control, events.Count,
            events.Count(e => e.Blocked));
        var effectiveness = effectivenessCalculator.Evaluate(measurement);
        if (!effectiveness.Success || effectiveness.Value is null)
            return Result<SecurityAnalysisReport>.Fail(effectiveness.Error ?? "MeasurementFailed");
        return Result<SecurityAnalysisReport>.Ok(new(cases.Value, effectiveness.Value));
    }
}
