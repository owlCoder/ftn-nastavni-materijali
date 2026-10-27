using Oib.Vezba06.Domain.Signals;
using Oib.Vezba06.Domain.Shared;
namespace Oib.Vezba06.Domain.Incidents;
public interface IIncidentFactory
{
    Result<Incident> Open(string? id, SecuritySignal? signal, DateTimeOffset openedAt);
}
