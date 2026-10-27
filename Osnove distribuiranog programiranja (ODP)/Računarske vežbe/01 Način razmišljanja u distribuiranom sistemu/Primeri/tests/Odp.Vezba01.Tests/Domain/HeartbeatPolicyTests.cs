using NUnit.Framework;
using Odp.Vezba01.Domain.Heartbeats;
using Odp.Vezba01.Domain.Nodes;

namespace Odp.Vezba01.Tests.Domain;

public sealed class HeartbeatPolicyTests
{
    private static readonly DateTimeOffset Known = new(2027, 2, 1, 9, 0, 0, TimeSpan.Zero);
    private static readonly StationNode Node = new("node-ns-1", "GS-NOVI-SAD", Known);

    private readonly HeartbeatPolicy _policy = new();

    [Test]
    public void Apply_WhenHeartbeatIsNewer_MovesKnownStateForward()
    {
        var (node, outcome) = _policy.Apply(Node, Known.AddSeconds(20));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(outcome, Is.EqualTo(new HeartbeatOutcome(true, HeartbeatCodes.Accepted)));
            Assert.That(node.LastHeartbeatAt, Is.EqualTo(Known.AddSeconds(20)));
        }
    }

    [TestCase(0)]
    [TestCase(-15)]
    public void Apply_WhenHeartbeatIsNotNewer_KeepsKnownState(int offsetSeconds)
    {
        var (node, outcome) = _policy.Apply(Node, Known.AddSeconds(offsetSeconds));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(outcome, Is.EqualTo(new HeartbeatOutcome(false, HeartbeatCodes.OutOfOrder)));
            Assert.That(node, Is.EqualTo(Node));
        }
    }
}
