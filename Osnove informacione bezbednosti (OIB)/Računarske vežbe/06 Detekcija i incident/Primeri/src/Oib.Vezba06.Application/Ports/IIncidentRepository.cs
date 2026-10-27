using Oib.Vezba06.Domain.Shared;
using Oib.Vezba06.Domain.Incidents;

namespace Oib.Vezba06.Application.Ports;

public interface IIncidentRepository
{
    Result<bool> Add(Incident? incident);
}
