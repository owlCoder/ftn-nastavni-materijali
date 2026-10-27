using Oib.Vezba08.Domain.Events;

namespace Oib.Vezba08.Application.Ports;

public interface ISecurityEventSource
{
    IReadOnlyCollection<SecurityEvent> GetEvents();
}
