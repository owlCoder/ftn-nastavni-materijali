using NUnit.Framework;
using Odp.Vezba07.Application.Flushing;
using Odp.Vezba07.Application.Recording;
using Odp.Vezba07.Domain.Inbox;
using Odp.Vezba07.Domain.Outbox;
using Odp.Vezba07.Infrastructure.Center;
using Odp.Vezba07.Infrastructure.Links;
using Odp.Vezba07.Infrastructure.Outbox;

namespace Odp.Vezba07.Tests.Application;

public sealed class OutboxFlowTests
{
    private InMemoryOutboxStore _outbox = null!;
    private CenterInbox _center = null!;
    private SimulatedUplink _uplink = null!;
    private RecordMeasurementHandler _record = null!;
    private FlushOutboxHandler _flush = null!;

    [SetUp]
    public void SetUp()
    {
        _outbox = new InMemoryOutboxStore();
        _center = new CenterInbox(new InboxFilter());
        _uplink = new SimulatedUplink(_center);
        _record = new RecordMeasurementHandler(_outbox);
        _flush = new FlushOutboxHandler(_outbox, new OutboxOrdering(), _uplink);
    }

    [Test]
    public void Record_WhenLinkIsDown_StillKeepsTheMessage()
    {
        _uplink.IsUp = false;

        var code = _record.Record("m-1", "a");
        var report = _flush.Flush();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(code, Is.EqualTo(OutboxCodes.Queued));
            Assert.That(report, Is.EqualTo(new FlushReport(0, 1, OutboxCodes.DeliveryUnconfirmed)));
            Assert.That(_center.AppliedPayloads, Is.Empty);
        }
    }

    [Test]
    public void Flush_AfterReconnect_DeliversEverythingInOriginalOrder()
    {
        _uplink.IsUp = false;
        _record.Record("m-1", "a");
        _record.Record("m-2", "b");
        _record.Record("m-3", "c");
        _uplink.IsUp = true;

        var report = _flush.Flush();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(report, Is.EqualTo(new FlushReport(3, 0, OutboxCodes.Flushed)));
            Assert.That(_center.AppliedPayloads, Is.EqualTo(new[] { "a", "b", "c" }));
        }
    }

    [Test]
    public void Flush_WhenAckIsLost_ResendsAndTheCenterAppliesTheMessageOnce()
    {
        _record.Record("m-1", "a");
        _record.Record("m-2", "b");
        _uplink.LoseNextAck = true;

        var first = _flush.Flush();
        var second = _flush.Flush();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(first, Is.EqualTo(new FlushReport(0, 2, OutboxCodes.DeliveryUnconfirmed)));
            Assert.That(second, Is.EqualTo(new FlushReport(2, 0, OutboxCodes.Flushed)));
            Assert.That(_center.AppliedPayloads, Is.EqualTo(new[] { "a", "b" }));
            Assert.That(_center.DuplicatesIgnored, Is.EqualTo(1));
        }
    }

    [Test]
    public void Record_WhenTheSameMessageIdIsRecordedTwice_KeepsOneEntry()
    {
        _record.Record("m-1", "a");

        var code = _record.Record("m-1", "a");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(code, Is.EqualTo(OutboxCodes.AlreadyQueued));
            Assert.That(_outbox.All(), Has.Count.EqualTo(1));
        }
    }

    [Test]
    public void Flush_WhenNothingIsPending_ReportsIt()
    {
        Assert.That(_flush.Flush(), Is.EqualTo(new FlushReport(0, 0, OutboxCodes.NothingPending)));
    }
}
