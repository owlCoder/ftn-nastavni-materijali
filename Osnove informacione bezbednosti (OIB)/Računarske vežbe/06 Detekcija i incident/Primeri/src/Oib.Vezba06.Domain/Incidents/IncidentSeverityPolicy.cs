using Oib.Vezba06.Domain.Signals;
using Oib.Vezba06.Domain.Shared;
namespace Oib.Vezba06.Domain.Incidents;
public sealed class IncidentSeverityPolicy(int highSeverityFrom) : IIncidentSeverityPolicy
{
    public Result<IncidentSeverity> For(SecuritySignal? signal)
    {
        if (highSeverityFrom < 1) return Result<IncidentSeverity>.Fail("InvalidSeverityConfiguration");
        if (signal is null || signal.Count < 1) return Result<IncidentSeverity>.Fail("InvalidSignal");
        return Result<IncidentSeverity>.Ok(signal.Count >= highSeverityFrom ? IncidentSeverity.High : IncidentSeverity.Medium);
    }
}
