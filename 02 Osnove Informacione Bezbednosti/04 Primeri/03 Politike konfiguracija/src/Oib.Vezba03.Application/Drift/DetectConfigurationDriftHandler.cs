using Oib.Vezba03.Application.Ports;
using Oib.Vezba03.Domain.Drift;

namespace Oib.Vezba03.Application.Drift;

public sealed class DetectConfigurationDriftHandler(
    ISecurityBaselineProvider baselineProvider,
    IActiveConfigurationProvider configurationProvider,
    ConfigurationDriftDetector detector,
    ICorrelationIdGenerator correlationIds,
    IDriftReportLog reportLog) : IDetectConfigurationDriftUseCase
{
    public DriftReport Detect()
    {
        var baseline = baselineProvider.GetBaseline();
        var findings = detector.Evaluate(baseline, configurationProvider.GetCurrent());

        var report = new DriftReport(
            correlationIds.NewCorrelationId(),
            baseline.Version,
            findings);

        reportLog.Record(report);
        return report;
    }
}
