using Oib.Vezba03.Application.Ports;
using Oib.Vezba03.Domain.Drift;
using Oib.Vezba03.Domain.Shared;

namespace Oib.Vezba03.Application.Drift;

public sealed class DetectConfigurationDriftHandler(
    ISecurityBaselineProvider baselineProvider,
    IActiveConfigurationProvider configurationProvider,
    IConfigurationDriftDetector detector,
    ICorrelationIdGenerator correlationIds,
    IDriftReportLog reportLog) : IDetectConfigurationDriftUseCase
{
    public Result<DriftReport> Detect()
    {
        var baseline = baselineProvider.GetBaseline();
        var findings = detector.Evaluate(baseline, configurationProvider.GetCurrent());

        if (!findings.Success || findings.Value is null)
            return Result<DriftReport>.Fail(findings.Error ?? "DetectionFailed");

        var report = new DriftReport(
            correlationIds.NewCorrelationId(),
            baseline.Version,
            findings.Value);

        var logged = reportLog.Record(report);
        if (!logged.Success) return Result<DriftReport>.Fail(logged.Error ?? "ReportWriteFailed");
        return Result<DriftReport>.Ok(report);
    }
}
