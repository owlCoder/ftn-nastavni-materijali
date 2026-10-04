using Odp.Vezba04.Application.Ports;
using Odp.Vezba04.Domain.Messaging;

namespace Odp.Vezba04.Infrastructure.Messaging;

public sealed class RecordingMessageProcessor : IMessageProcessor
{
    private readonly List<string> _processedPayloads = [];

    public IReadOnlyList<string> ProcessedPayloads => _processedPayloads;

    public void Process(MessageEnvelope envelope) => _processedPayloads.Add(envelope.Payload);
}
