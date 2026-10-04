namespace Odp.Vezba01.Domain.Liveness;

/// <summary>Ono što sistem zna o čvoru: koliko dugo ćuti, ne da li je "mrtav".</summary>
public sealed record NodeLivenessAssessment(NodeStatus Status, string Code, TimeSpan Silence);
