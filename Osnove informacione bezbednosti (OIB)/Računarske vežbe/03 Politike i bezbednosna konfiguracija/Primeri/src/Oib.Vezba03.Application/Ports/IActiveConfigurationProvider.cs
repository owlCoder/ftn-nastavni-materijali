using Oib.Vezba03.Domain.Configuration;

namespace Oib.Vezba03.Application.Ports;

public interface IActiveConfigurationProvider
{
    SecurityConfiguration GetCurrent();
}
