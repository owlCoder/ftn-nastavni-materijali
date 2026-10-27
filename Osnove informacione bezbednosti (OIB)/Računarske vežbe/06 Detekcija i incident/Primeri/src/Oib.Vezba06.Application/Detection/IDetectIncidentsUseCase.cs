using Oib.Vezba06.Domain.Incidents;
using Oib.Vezba06.Domain.Shared;
namespace Oib.Vezba06.Application.Detection;
public interface IDetectIncidentsUseCase
{
    Result<IReadOnlyList<Incident>> Detect();
}
