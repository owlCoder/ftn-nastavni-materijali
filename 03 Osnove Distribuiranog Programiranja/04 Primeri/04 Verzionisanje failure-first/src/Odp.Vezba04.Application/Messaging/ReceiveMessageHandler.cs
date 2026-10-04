using Odp.Vezba04.Application.Ports;
using Odp.Vezba04.Domain.Messaging;

namespace Odp.Vezba04.Application.Messaging;

public sealed class ReceiveMessageHandler(
    InboxPolicy policy,
    IProcessedMessageStore processedMessages,
    IMessageProcessor processor,
    IClock clock) : IReceiveMessageUseCase
{
    public InboxDecision Receive(MessageEnvelope envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        var decision = policy.Decide(
            envelope,
            processedMessages.Contains(envelope.MessageId),
            clock.UtcNow);

        if (decision.Process)
        {
            processor.Process(envelope);
            processedMessages.Add(envelope.MessageId);
        }

        return decision;
    }
}
