using Odp.Vezba07.Application.Flushing;
using Odp.Vezba07.Application.Recording;
using Odp.Vezba07.Domain.Inbox;
using Odp.Vezba07.Domain.Outbox;
using Odp.Vezba07.Infrastructure.Center;
using Odp.Vezba07.Infrastructure.Links;
using Odp.Vezba07.Infrastructure.Outbox;

namespace Odp.Vezba07.ConsoleUi;

public static class CompositionRoot
{
    public static OutboxDemo CreateDemo(TextWriter output)
    {
        var outbox = new InMemoryOutboxStore();
        var center = new CenterInbox(new InboxFilter());
        var uplink = new SimulatedUplink(center);

        return new OutboxDemo(
            new RecordMeasurementHandler(outbox),
            new FlushOutboxHandler(outbox, new OutboxOrdering(), uplink),
            uplink,
            center,
            output);
    }
}
