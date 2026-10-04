using NUnit.Framework;
using Odp.Vezba01.Domain.Liveness;
using Odp.Vezba01.Domain.Nodes;

namespace Odp.Vezba01.Tests.Domain;

public sealed class NodeLivenessPolicyTests
{
    private static readonly DateTimeOffset Now = new(2027, 2, 1, 9, 0, 0, TimeSpan.Zero);

    private readonly NodeLivenessPolicy _policy =
        new(new LivenessThresholds(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(90)));

    [TestCase(0, NodeStatus.Online, NodeLivenessCodes.HeartbeatFresh)]
    [TestCase(29, NodeStatus.Online, NodeLivenessCodes.HeartbeatFresh)]
    [TestCase(30, NodeStatus.Suspected, NodeLivenessCodes.HeartbeatLate)]
    [TestCase(89, NodeStatus.Suspected, NodeLivenessCodes.HeartbeatLate)]
    [TestCase(90, NodeStatus.Unreachable, NodeLivenessCodes.HeartbeatMissing)]
    public void Evaluate_ReportsStatusFromLengthOfSilence(int silenceSeconds, NodeStatus status, string code)
    {
        var assessment = _policy.Evaluate(NodeSeenAt(Now.AddSeconds(-silenceSeconds)), Now);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(assessment.Status, Is.EqualTo(status));
            Assert.That(assessment.Code, Is.EqualTo(code));
            Assert.That(assessment.Silence, Is.EqualTo(TimeSpan.FromSeconds(silenceSeconds)));
        }
    }

    [Test]
    public void Evaluate_WhenHeartbeatIsAheadOfLocalClock_TreatsSilenceAsZero()
    {
        var assessment = _policy.Evaluate(NodeSeenAt(Now.AddSeconds(5)), Now);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(assessment.Status, Is.EqualTo(NodeStatus.Online));
            Assert.That(assessment.Silence, Is.EqualTo(TimeSpan.Zero));
        }
    }

    [Test]
    public void Constructor_WhenUnreachableThresholdIsNotAfterSuspectThreshold_Rejects()
    {
        var thresholds = new LivenessThresholds(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));

        Action create = () => _ = new NodeLivenessPolicy(thresholds);

        Assert.That(create, Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    private static StationNode NodeSeenAt(DateTimeOffset lastHeartbeatAt) =>
        new("node-ns-1", "GS-NOVI-SAD", lastHeartbeatAt);
}
