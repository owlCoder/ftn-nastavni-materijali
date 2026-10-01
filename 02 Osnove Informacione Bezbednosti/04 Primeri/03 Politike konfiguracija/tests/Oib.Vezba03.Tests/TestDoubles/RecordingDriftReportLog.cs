using Oib.Vezba03.Application.Drift;
using Oib.Vezba03.Application.Ports;

namespace Oib.Vezba03.Tests.TestDoubles;

internal sealed class RecordingDriftReportLog : IDriftReportLog
{
    public List<DriftReport> Reports { get; } = [];

    public void Record(DriftReport report) => Reports.Add(report);
}
