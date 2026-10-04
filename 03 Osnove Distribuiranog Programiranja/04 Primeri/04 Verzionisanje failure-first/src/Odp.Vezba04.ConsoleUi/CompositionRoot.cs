using Odp.Vezba04.Application.Messaging;
using Odp.Vezba04.Domain.Messaging;
using Odp.Vezba04.Infrastructure.Messaging;
using Odp.Vezba04.Infrastructure.Time;

namespace Odp.Vezba04.ConsoleUi;

public static class CompositionRoot
{
    public static InboxDemo CreateDemo(TextWriter output)
    {
        var clock = new ManualClock(DemoData.Start);
        var processor = new RecordingMessageProcessor();
        var receive = new ReceiveMessageHandler(
            new InboxPolicy(DemoData.Rules),
            new InMemoryProcessedMessageStore(),
            processor,
            clock);

        return new InboxDemo(receive, processor, clock, output);
    }
}
