namespace Odp.Vezba02.Application.Ingestion;

public interface IIngestSimulatedTelemetryUseCase
{
    IngestionReport Run(int messageCount);
}
