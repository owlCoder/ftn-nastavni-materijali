using Oib.Vezba06.Domain.Incidents;

namespace Oib.Vezba06.Application.Detection;

public interface IDetectIncidentsUseCase
{
    IReadOnlyList<Incident> Detect();
}
