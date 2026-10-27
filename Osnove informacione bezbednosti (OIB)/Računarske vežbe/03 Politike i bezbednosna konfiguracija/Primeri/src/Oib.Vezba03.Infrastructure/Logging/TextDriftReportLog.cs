using Oib.Vezba03.Application.Drift;
using Oib.Vezba03.Application.Ports;

namespace Oib.Vezba03.Infrastructure.Logging;

public sealed class TextDriftReportLog(TextWriter output) : IDriftReportLog
{
    public void Record(DriftReport report)
    {
        ArgumentNullException.ThrowIfNull(report);

        if (report.IsCompliant)
        {
            output.WriteLine(
                $"{report.CorrelationId} | baseline={report.BaselineVersion} | compliant");
            return;
        }

        foreach (var finding in report.Findings)
            output.WriteLine(
                $"{report.CorrelationId} | baseline={report.BaselineVersion} | " +
                $"{finding.Control} | expected={finding.Expected} actual={finding.Actual}");
    }
}
