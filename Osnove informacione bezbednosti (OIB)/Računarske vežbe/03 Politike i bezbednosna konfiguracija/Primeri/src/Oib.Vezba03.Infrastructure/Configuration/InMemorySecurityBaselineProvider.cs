using Oib.Vezba03.Application.Ports;
using Oib.Vezba03.Domain.Configuration;

namespace Oib.Vezba03.Infrastructure.Configuration;

public sealed class InMemorySecurityBaselineProvider(SecurityBaseline baseline)
    : ISecurityBaselineProvider
{
    public SecurityBaseline GetBaseline() => baseline;
}
