using Oib.Vezba03.Domain.Configuration;

namespace Oib.Vezba03.Domain.Drift;

public interface IConfigurationControl
{
    ConfigurationFinding? Check(SecurityBaseline baseline, SecurityConfiguration current);
}
