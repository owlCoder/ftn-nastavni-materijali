namespace Odp.Vezba07.Domain.Outbox;

public sealed class OutboxOrdering
{
    /// <summary>Neisporučene poruke idu redom kojim su nastale; preskakanje bi promenilo značenje.</summary>
    public IReadOnlyList<OutboxMessage> PendingInOrder(IEnumerable<OutboxMessage> messages)
    {
        ArgumentNullException.ThrowIfNull(messages);

        return messages
            .Where(message => !message.Delivered)
            .OrderBy(message => message.Position)
            .ToArray();
    }
}
