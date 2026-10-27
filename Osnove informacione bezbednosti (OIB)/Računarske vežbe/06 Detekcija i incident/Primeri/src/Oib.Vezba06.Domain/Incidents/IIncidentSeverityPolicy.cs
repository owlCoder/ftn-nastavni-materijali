using Oib.Vezba06.Domain.Signals;
using Oib.Vezba06.Domain.Shared;
namespace Oib.Vezba06.Domain.Incidents;
public interface IIncidentSeverityPolicy
{
    Result<IncidentSeverity> For(SecuritySignal? signal);
}
