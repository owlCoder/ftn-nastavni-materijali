using Odp.Vezba01.Domain.Nodes;

namespace Odp.Vezba01.Domain.Liveness;

public sealed class NodeLivenessPolicy
{
    private readonly LivenessThresholds _thresholds;

    public NodeLivenessPolicy(LivenessThresholds thresholds)
    {
        ArgumentNullException.ThrowIfNull(thresholds);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(thresholds.SuspectAfter, TimeSpan.Zero);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(
            thresholds.UnreachableAfter,
            thresholds.SuspectAfter);
        _thresholds = thresholds;
    }

    public NodeLivenessAssessment Evaluate(StationNode node, DateTimeOffset now)
    {
        ArgumentNullException.ThrowIfNull(node);

        var silence = SilenceOf(node, now);
        if (silence < _thresholds.SuspectAfter)
            return new(NodeStatus.Online, NodeLivenessCodes.HeartbeatFresh, silence);
        if (silence < _thresholds.UnreachableAfter)
            return new(NodeStatus.Suspected, NodeLivenessCodes.HeartbeatLate, silence);
        return new(NodeStatus.Unreachable, NodeLivenessCodes.HeartbeatMissing, silence);
    }

    private static TimeSpan SilenceOf(StationNode node, DateTimeOffset now) =>
        now > node.LastHeartbeatAt ? now - node.LastHeartbeatAt : TimeSpan.Zero;
}
