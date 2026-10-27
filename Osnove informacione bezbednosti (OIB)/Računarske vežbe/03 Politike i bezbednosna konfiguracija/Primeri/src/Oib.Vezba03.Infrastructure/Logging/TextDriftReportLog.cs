using Oib.Vezba03.Application.Drift;
using Oib.Vezba03.Application.Ports;
using Oib.Vezba03.Domain.Shared;
namespace Oib.Vezba03.Infrastructure.Logging;
public sealed class TextDriftReportLog(TextWriter output) : IDriftReportLog
{
    public Result<bool> Record(DriftReport? report)
    {
        if (report is null) return Result<bool>.Fail("InvalidReport");
        try
        {
            if (report.IsCompliant)
                output.WriteLine($"{report.CorrelationId} | baseline={report.BaselineVersion} | compliant");
            else foreach (var finding in report.Findings)
                output.WriteLine($"{report.CorrelationId} | baseline={report.BaselineVersion} | " +
                    $"{finding.Control} | expected={finding.Expected} actual={finding.Actual}");
            return Result<bool>.Ok(true);
        }
        catch (IOException) { return Result<bool>.Fail("ReportWriteFailed"); }
        catch (ObjectDisposedException) { return Result<bool>.Fail("ReportWriteFailed"); }
    }
}
