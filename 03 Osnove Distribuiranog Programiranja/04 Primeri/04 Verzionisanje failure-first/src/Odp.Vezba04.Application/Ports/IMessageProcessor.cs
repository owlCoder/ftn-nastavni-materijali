using Odp.Vezba04.Domain.Messaging;

namespace Odp.Vezba04.Application.Ports;

/// <summary>Poslovni efekat poruke; sme da se dogodi najviše jednom po poruci.</summary>
public interface IMessageProcessor
{
    void Process(MessageEnvelope envelope);
}
