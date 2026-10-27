using Odp.Vezba01.Domain.Nodes;

namespace Odp.Vezba01.Domain.Heartbeats;

public sealed class HeartbeatPolicy
{
    /// <summary>Poruka koja je zakasnila ne sme da vrati poznato stanje unazad.</summary>
    public (StationNode Node, HeartbeatOutcome Outcome) Apply(StationNode node, DateTimeOffset sentAt)
    {
        ArgumentNullException.ThrowIfNull(node);

        return sentAt > node.LastHeartbeatAt
            ? (node with { LastHeartbeatAt = sentAt }, new(true, HeartbeatCodes.Accepted))
            : (node, new(false, HeartbeatCodes.OutOfOrder));
    }
}
