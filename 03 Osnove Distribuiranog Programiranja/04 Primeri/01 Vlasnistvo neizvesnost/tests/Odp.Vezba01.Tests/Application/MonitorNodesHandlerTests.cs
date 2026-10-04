using NUnit.Framework;
using Odp.Vezba01.Application.Monitoring;
using Odp.Vezba01.Domain.Liveness;
using Odp.Vezba01.Domain.Nodes;
using Odp.Vezba01.Infrastructure.Nodes;
using Odp.Vezba01.Infrastructure.Time;

namespace Odp.Vezba01.Tests.Application;

public sealed class MonitorNodesHandlerTests
{
    private static readonly DateTimeOffset Start = new(2027, 2, 1, 9, 0, 0, TimeSpan.Zero);

    [Test]
    public void Assess_WhenNodeStaysSilent_ReportsItUnreachableButKeepsItRegistered()
    {
        var clock = new ManualClock(Start);
        var registry = new InMemoryNodeRegistry([new StationNode("node-ns-1", "GS-NOVI-SAD", Start)]);
        var handler = new MonitorNodesHandler(
            registry,
            new NodeLivenessPolicy(new LivenessThresholds(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(90))),
            clock);

        clock.Advance(TimeSpan.FromMinutes(10));
        var view = handler.Assess().Single();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(view.Liveness.Status, Is.EqualTo(NodeStatus.Unreachable));
            Assert.That(view.Liveness.Code, Is.EqualTo(NodeLivenessCodes.HeartbeatMissing));
            Assert.That(registry.Find("node-ns-1"), Is.Not.Null);
        }
    }
}
