using Odp.Vezba05.Domain.Telemetry;

namespace Odp.Vezba05.Domain.ReadModels;

public sealed class StationViewProjector
{
    /// <summary>Starije merenje koje stigne kasnije ne sme da pregazi noviji prikaz.</summary>
    public (StationView View, bool Changed) Apply(StationView? current, TelemetryReading reading)
    {
        ArgumentNullException.ThrowIfNull(reading);

        if (current is not null && reading.Sequence <= current.LastSequence)
            return (current, false);

        return (
            new StationView(
                reading.StationId,
                reading.Sequence,
                reading.MeasuredAt,
                reading.SignalStrengthDbm),
            true);
    }
}
