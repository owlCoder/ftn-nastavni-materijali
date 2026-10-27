using Odp.Vezba05.Application.Ingestion;
using Odp.Vezba05.Application.Ports;
using Odp.Vezba05.Application.Queries;
using Odp.Vezba05.Domain.Telemetry;
using Odp.Vezba05.Infrastructure.Time;

namespace Odp.Vezba05.ConsoleUi;

public sealed class TelemetryDemo(
    IIngestTelemetryUseCase ingest,
    IGetStationStatusUseCase getStatus,
    ITelemetryLog log,
    ManualClock clock,
    TextWriter output)
{
    public void Run()
    {
        Ingest(DemoData.Reading(1, -91));
        Ingest(DemoData.Reading(3, -88));
        Ingest(DemoData.Reading(2, -97));
        Ingest(DemoData.Reading(0, -90));
        ShowStatus("posle prijema");

        clock.Advance(TimeSpan.FromMinutes(2));
        ShowStatus("dva minuta bez novih merenja");

        output.WriteLine();
        output.WriteLine($"Zapis sadrži {log.ForStation(DemoData.Station).Count} merenja, prikaz samo poslednje.");
    }

    private void Ingest(TelemetryReading reading)
    {
        var outcome = ingest.Ingest(reading);
        output.WriteLine($"merenje #{reading.Sequence} -> {outcome.Code}, prikaz promenjen: {outcome.ViewChanged}");
    }

    private void ShowStatus(string label)
    {
        var status = getStatus.Get(DemoData.Station);
        output.WriteLine(status is null
            ? $"[{label}] nema prikaza"
            : $"[{label}] #{status.View.LastSequence} {status.View.LastSignalStrengthDbm} dBm, " +
              $"staro {status.Age.TotalSeconds:0} s, zastarelo: {status.IsStale}");
    }
}
