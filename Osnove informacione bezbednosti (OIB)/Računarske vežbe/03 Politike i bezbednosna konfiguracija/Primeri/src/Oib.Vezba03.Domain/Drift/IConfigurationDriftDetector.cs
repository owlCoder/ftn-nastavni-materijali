using Oib.Vezba03.Domain.Configuration;
using Oib.Vezba03.Domain.Shared;
namespace Oib.Vezba03.Domain.Drift;
public interface IConfigurationDriftDetector
{
    Result<IReadOnlyList<ConfigurationFinding>> Evaluate(SecurityBaseline? baseline, SecurityConfiguration? current);
}
