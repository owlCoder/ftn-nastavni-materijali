using Oib.Vezba03.Domain.Shared;
using Oib.Vezba03.Application.Drift;
using Oib.Vezba03.Application.Ports;

namespace Oib.Vezba03.Tests.TestDoubles;

internal sealed class RecordingDriftReportLog : IDriftReportLog
{
    public List<DriftReport> Reports { get; } = [];

    public Result<bool> Record(DriftReport? report)
    {
        if (report is null) return Result<bool>.Fail("InvalidReport");
        Reports.Add(report);
        return Result<bool>.Ok(true);
    }
}
