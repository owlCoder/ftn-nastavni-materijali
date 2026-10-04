using Odp.Vezba05.Domain.Telemetry;

namespace Odp.Vezba05.Application.Ingestion;

public interface IIngestTelemetryUseCase
{
    IngestOutcome Ingest(TelemetryReading reading);
}
