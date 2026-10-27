using Oib.Vezba03.Application.Ports;
using Oib.Vezba03.Domain.Configuration;

namespace Oib.Vezba03.Infrastructure.Configuration;

public sealed class InMemoryActiveConfigurationProvider(SecurityConfiguration current)
    : IActiveConfigurationProvider
{
    public SecurityConfiguration GetCurrent() => current;
}
