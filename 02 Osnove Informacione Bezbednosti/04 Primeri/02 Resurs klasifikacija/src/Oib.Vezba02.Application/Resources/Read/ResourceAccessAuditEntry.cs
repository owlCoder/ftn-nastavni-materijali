namespace Oib.Vezba02.Application.Resources.Read;

public sealed record ResourceAccessAuditEntry(
    string ActorId,
    string ResourceId,
    ReadResourceOutcome Outcome,
    string Code);
