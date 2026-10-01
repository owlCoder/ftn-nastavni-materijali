using Oib.Vezba03.Application.Drift;

namespace Oib.Vezba03.Application.Ports;

public interface IDriftReportLog
{
    void Record(DriftReport report);
}
