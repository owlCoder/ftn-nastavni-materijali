namespace Odp.Vezba01.Domain.Nodes;

/// <summary>Poslovni entitet: postoji i kada proces koji ga predstavlja ćuti.</summary>
public sealed record StationNode(string NodeId, string StationId, DateTimeOffset LastHeartbeatAt);
