using Odp.Vezba04.Domain.Messaging;

namespace Odp.Vezba04.Application.Messaging;

public interface IReceiveMessageUseCase
{
    InboxDecision Receive(MessageEnvelope envelope);
}
