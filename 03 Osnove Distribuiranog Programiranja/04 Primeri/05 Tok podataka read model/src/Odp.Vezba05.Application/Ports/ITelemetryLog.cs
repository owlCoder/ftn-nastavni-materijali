using Odp.Vezba05.Domain.Telemetry;

namespace Odp.Vezba05.Application.Ports;

/// <summary>Autoritativan zapis svih prihvaćenih merenja, redom kojim su stigla.</summary>
public interface ITelemetryLog
{
    void Append(TelemetryReading reading);

    IReadOnlyList<TelemetryReading> ForStation(string stationId);
}
