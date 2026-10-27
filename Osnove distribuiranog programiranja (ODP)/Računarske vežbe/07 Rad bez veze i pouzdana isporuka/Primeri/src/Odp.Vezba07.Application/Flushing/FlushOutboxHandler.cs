using Odp.Vezba07.Application.Ports;
using Odp.Vezba07.Domain.Outbox;

namespace Odp.Vezba07.Application.Flushing;

public sealed class FlushOutboxHandler(IOutboxStore outbox, OutboxOrdering ordering, IUplink uplink)
    : IFlushOutboxUseCase
{
    public FlushReport Flush()
    {
        var pending = ordering.PendingInOrder(outbox.All());
        if (pending.Count == 0)
            return new(0, 0, OutboxCodes.NothingPending);

        var delivered = 0;
        foreach (var message in pending)
        {
            if (!uplink.TrySend(message))
                return new(delivered, pending.Count - delivered, OutboxCodes.DeliveryUnconfirmed);

            outbox.MarkDelivered(message.Position);
            delivered++;
        }

        return new(delivered, 0, OutboxCodes.Flushed);
    }
}
