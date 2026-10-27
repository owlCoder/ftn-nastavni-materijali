using Odp.Vezba07.Application.Ports;
using Odp.Vezba07.Domain.Outbox;

namespace Odp.Vezba07.Infrastructure.Outbox;

public sealed class InMemoryOutboxStore : IOutboxStore
{
    private readonly List<OutboxMessage> _messages = [];

    public OutboxMessage? Find(string messageId) =>
        _messages.FirstOrDefault(message => message.MessageId == messageId);

    public OutboxMessage Append(string messageId, string payload)
    {
        var message = new OutboxMessage(_messages.Count + 1, messageId, payload, Delivered: false);
        _messages.Add(message);
        return message;
    }

    public IReadOnlyList<OutboxMessage> All() => _messages.ToArray();

    public void MarkDelivered(long position)
    {
        var index = _messages.FindIndex(message => message.Position == position);
        _messages[index] = _messages[index] with { Delivered = true };
    }
}
