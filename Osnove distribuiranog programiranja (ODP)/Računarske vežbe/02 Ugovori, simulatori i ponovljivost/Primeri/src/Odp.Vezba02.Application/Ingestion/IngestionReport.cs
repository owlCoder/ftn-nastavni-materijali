namespace Odp.Vezba02.Application.Ingestion;

public sealed record IngestionReport(int Accepted, IReadOnlyList<RejectedMessage> Rejected);
