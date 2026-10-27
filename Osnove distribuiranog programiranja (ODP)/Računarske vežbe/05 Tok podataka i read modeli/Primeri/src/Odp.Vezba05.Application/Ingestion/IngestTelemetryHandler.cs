using Odp.Vezba05.Application.Ports;
using Odp.Vezba05.Domain.ReadModels;
using Odp.Vezba05.Domain.Telemetry;

namespace Odp.Vezba05.Application.Ingestion;

public sealed class IngestTelemetryHandler(
    TelemetryReadingValidator validator,
    ITelemetryLog log,
    StationViewProjector projector,
    IStationViewStore views) : IIngestTelemetryUseCase
{
    public IngestOutcome Ingest(TelemetryReading reading)
    {
        ArgumentNullException.ThrowIfNull(reading);

        var validation = validator.Validate(reading);
        if (!validation.Success)
            return new(false, validation.Error!, ViewChanged: false);

        log.Append(reading);

        var (view, changed) = projector.Apply(views.Find(reading.StationId), reading);
        if (changed)
            views.Save(view);

        return new(true, TelemetryCodes.Stored, changed);
    }
}
