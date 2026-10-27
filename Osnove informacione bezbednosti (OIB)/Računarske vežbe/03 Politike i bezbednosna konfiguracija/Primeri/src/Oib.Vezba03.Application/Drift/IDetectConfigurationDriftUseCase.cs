using Oib.Vezba03.Domain.Shared;
namespace Oib.Vezba03.Application.Drift;
public interface IDetectConfigurationDriftUseCase
{
    Result<DriftReport> Detect();
}
