using Odp.Vezba04.Application.Messaging;
using Odp.Vezba04.Domain.Messaging;
using Odp.Vezba04.Infrastructure.Messaging;
using Odp.Vezba04.Infrastructure.Time;

namespace Odp.Vezba04.ConsoleUi;

public sealed class InboxDemo(
    IReceiveMessageUseCase receive,
    RecordingMessageProcessor processor,
    ManualClock clock,
    TextWriter output)
{
    public void Run()
    {
        var first = DemoData.Message("m-1", 1, 0, clock.UtcNow);

        Show("prva isporuka", first);
        Show("ponovljena isporuka iste poruke", first);
        Show("novija minor verzija", DemoData.Message("m-2", 1, 3, clock.UtcNow));
        Show("druga major verzija", DemoData.Message("m-3", 2, 0, clock.UtcNow));

        var delayed = DemoData.Message("m-4", 1, 0, clock.UtcNow);
        clock.Advance(TimeSpan.FromMinutes(12));
        Show("poruka koja je putovala 12 minuta", delayed);

        output.WriteLine();
        output.WriteLine($"Poslovni efekat se dogodio {processor.ProcessedPayloads.Count} puta:");
        foreach (var payload in processor.ProcessedPayloads)
            output.WriteLine($"  {payload}");
    }

    private void Show(string label, MessageEnvelope envelope)
    {
        var decision = receive.Receive(envelope);
        output.WriteLine($"{envelope.MessageId} v{envelope.Version} ({label}) -> {decision.Code}");
    }
}
