namespace Odp.Vezba01.Domain.Liveness;

public sealed record LivenessThresholds(TimeSpan SuspectAfter, TimeSpan UnreachableAfter);
