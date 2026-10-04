namespace Odp.Vezba05.Application.Ingestion;

public sealed record IngestOutcome(bool Stored, string Code, bool ViewChanged);
