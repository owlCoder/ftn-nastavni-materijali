using Oib.Vezba08.Application.Ports;
using Oib.Vezba08.Domain.Events;

namespace Oib.Vezba08.Infrastructure.Events;

public sealed class InMemorySecurityEventSource(IEnumerable<SecurityEvent> events)
    : ISecurityEventSource
{
    private readonly IReadOnlyCollection<SecurityEvent> _events = events.ToArray();

    public IReadOnlyCollection<SecurityEvent> GetEvents() => _events;
}
