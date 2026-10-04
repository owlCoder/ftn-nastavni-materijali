using NUnit.Framework;
using Odp.Vezba07.Domain.Outbox;

namespace Odp.Vezba07.Tests.Domain;

public sealed class OutboxOrderingTests
{
    private readonly OutboxOrdering _ordering = new();

    [Test]
    public void PendingInOrder_SkipsDeliveredMessagesAndKeepsCreationOrder()
    {
        OutboxMessage[] messages =
        [
            new(3, "m-3", "c", Delivered: false),
            new(1, "m-1", "a", Delivered: true),
            new(2, "m-2", "b", Delivered: false)
        ];

        var pending = _ordering.PendingInOrder(messages);

        Assert.That(pending.Select(message => message.MessageId), Is.EqualTo(new[] { "m-2", "m-3" }));
    }
}
