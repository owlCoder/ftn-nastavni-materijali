using Oib.Vezba03.Domain.Shared;
using Oib.Vezba03.Application.Drift;

namespace Oib.Vezba03.Application.Ports;

public interface IDriftReportLog
{
    Result<bool> Record(DriftReport? report);
}
