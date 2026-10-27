using NUnit.Framework;
using Odp.Vezba04.Application.Messaging;
using Odp.Vezba04.Domain.Messaging;
using Odp.Vezba04.Infrastructure.Messaging;
using Odp.Vezba04.Infrastructure.Time;

namespace Odp.Vezba04.Tests.Application;

public sealed class ReceiveMessageHandlerTests
{
    private static readonly DateTimeOffset Start = new(2027, 2, 22, 9, 0, 0, TimeSpan.Zero);

    private ManualClock _clock = null!;
    private RecordingMessageProcessor _processor = null!;
    private ReceiveMessageHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _clock = new ManualClock(Start);
        _processor = new RecordingMessageProcessor();
        _handler = new ReceiveMessageHandler(
            new InboxPolicy(new InboxRules(SupportedMajor: 1, TimeSpan.FromMinutes(5))),
            new InMemoryProcessedMessageStore(),
            _processor,
            _clock);
    }

    [Test]
    public void Receive_WhenTheSameMessageIsDeliveredTwice_ProcessesItOnce()
    {
        var envelope = Envelope("m-1", 1, Start);

        var first = _handler.Receive(envelope);
        var second = _handler.Receive(envelope);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(first.Code, Is.EqualTo(InboxCodes.Accepted));
            Assert.That(second.Code, Is.EqualTo(InboxCodes.Duplicate));
            Assert.That(_processor.ProcessedPayloads, Has.Count.EqualTo(1));
        }
    }

    [Test]
    public void Receive_WhenMessageArrivesTooLate_DoesNotProcessIt()
    {
        var envelope = Envelope("m-1", 1, Start);
        _clock.Advance(TimeSpan.FromMinutes(12));

        var decision = _handler.Receive(envelope);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Code, Is.EqualTo(InboxCodes.TooOld));
            Assert.That(_processor.ProcessedPayloads, Is.Empty);
        }
    }

    [Test]
    public void Receive_WhenMessageWasRejected_ALaterValidDeliveryIsStillProcessed()
    {
        _handler.Receive(Envelope("m-1", 2, Start));

        var decision = _handler.Receive(Envelope("m-1", 1, Start));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Code, Is.EqualTo(InboxCodes.Accepted));
            Assert.That(_processor.ProcessedPayloads, Has.Count.EqualTo(1));
        }
    }

    private static MessageEnvelope Envelope(string id, int major, DateTimeOffset sentAt) =>
        new(id, new ContractVersion(major, 0), sentAt, $"payload {id}");
}
