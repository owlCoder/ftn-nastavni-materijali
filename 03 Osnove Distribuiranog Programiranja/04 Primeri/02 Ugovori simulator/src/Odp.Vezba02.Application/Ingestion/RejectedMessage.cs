namespace Odp.Vezba02.Application.Ingestion;

public sealed record RejectedMessage(long Sequence, string Code);
