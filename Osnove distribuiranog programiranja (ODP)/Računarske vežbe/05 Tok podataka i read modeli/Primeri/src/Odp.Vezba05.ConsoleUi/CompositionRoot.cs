using Odp.Vezba05.Application.Ingestion;
using Odp.Vezba05.Application.Queries;
using Odp.Vezba05.Domain.ReadModels;
using Odp.Vezba05.Domain.Telemetry;
using Odp.Vezba05.Infrastructure.ReadModels;
using Odp.Vezba05.Infrastructure.Telemetry;
using Odp.Vezba05.Infrastructure.Time;

namespace Odp.Vezba05.ConsoleUi;

public static class CompositionRoot
{
    public static TelemetryDemo CreateDemo(TextWriter output)
    {
        var clock = new ManualClock(DemoData.Start.AddSeconds(20));
        var log = new InMemoryTelemetryLog();
        var views = new InMemoryStationViewStore();

        return new TelemetryDemo(
            new IngestTelemetryHandler(new TelemetryReadingValidator(), log, new StationViewProjector(), views),
            new GetStationStatusHandler(views, new FreshnessPolicy(DemoData.StaleAfter), clock),
            log,
            clock,
            output);
    }
}
