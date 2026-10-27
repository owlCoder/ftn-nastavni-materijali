using Odp.Vezba07.Domain.Outbox;

namespace Odp.Vezba07.Application.Ports;

public interface IOutboxStore
{
    OutboxMessage? Find(string messageId);

    OutboxMessage Append(string messageId, string payload);

    IReadOnlyList<OutboxMessage> All();

    void MarkDelivered(long position);
}
