using Odp.Vezba05.Application.Ports;
using Odp.Vezba05.Domain.Telemetry;

namespace Odp.Vezba05.Infrastructure.Telemetry;

public sealed class InMemoryTelemetryLog : ITelemetryLog
{
    private readonly List<TelemetryReading> _readings = [];

    public void Append(TelemetryReading reading) => _readings.Add(reading);

    public IReadOnlyList<TelemetryReading> ForStation(string stationId) =>
        _readings.Where(reading => reading.StationId == stationId).ToArray();
}
