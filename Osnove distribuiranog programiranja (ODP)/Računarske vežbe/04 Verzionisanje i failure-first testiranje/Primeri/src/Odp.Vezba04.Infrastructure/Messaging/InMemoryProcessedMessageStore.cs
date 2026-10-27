using Odp.Vezba04.Application.Ports;

namespace Odp.Vezba04.Infrastructure.Messaging;

public sealed class InMemoryProcessedMessageStore : IProcessedMessageStore
{
    private readonly HashSet<string> _messageIds = new(StringComparer.Ordinal);

    public bool Contains(string messageId) => _messageIds.Contains(messageId);

    public void Add(string messageId) => _messageIds.Add(messageId);
}
