using Oib.Vezba06.Domain.Incidents;

namespace Oib.Vezba06.Application.Ports;

public interface IIncidentRepository
{
    void Add(Incident incident);
}
