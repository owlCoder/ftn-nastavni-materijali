using NUnit.Framework;
using Odp.Vezba01.Application.Heartbeats;
using Odp.Vezba01.Domain.Heartbeats;
using Odp.Vezba01.Domain.Nodes;
using Odp.Vezba01.Infrastructure.Nodes;

namespace Odp.Vezba01.Tests.Application;

public sealed class ReportHeartbeatHandlerTests
{
    private static readonly DateTimeOffset Known = new(2027, 2, 1, 9, 0, 0, TimeSpan.Zero);

    private InMemoryNodeRegistry _registry = null!;
    private ReportHeartbeatHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _registry = new InMemoryNodeRegistry([new StationNode("node-ns-1", "GS-NOVI-SAD", Known)]);
        _handler = new ReportHeartbeatHandler(_registry, new HeartbeatPolicy());
    }

    [Test]
    public void Report_WhenHeartbeatIsNewer_StoresIt()
    {
        var outcome = _handler.Report("node-ns-1", Known.AddSeconds(30));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(outcome.Accepted, Is.True);
            Assert.That(_registry.Find("node-ns-1")!.LastHeartbeatAt, Is.EqualTo(Known.AddSeconds(30)));
        }
    }

    [Test]
    public void Report_WhenHeartbeatArrivesLate_DoesNotMoveStateBackwards()
    {
        _handler.Report("node-ns-1", Known.AddSeconds(30));

        var outcome = _handler.Report("node-ns-1", Known.AddSeconds(10));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(outcome.Code, Is.EqualTo(HeartbeatCodes.OutOfOrder));
            Assert.That(_registry.Find("node-ns-1")!.LastHeartbeatAt, Is.EqualTo(Known.AddSeconds(30)));
        }
    }

    [Test]
    public void Report_WhenNodeIsNotRegistered_DoesNotCreateIt()
    {
        var outcome = _handler.Report("node-x", Known);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(outcome, Is.EqualTo(new HeartbeatOutcome(false, HeartbeatCodes.NodeUnknown)));
            Assert.That(_registry.Find("node-x"), Is.Null);
        }
    }
}
