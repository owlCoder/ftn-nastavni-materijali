namespace Odp.Vezba05.Domain.Telemetry;

public sealed record TelemetryReading(
    string StationId,
    long Sequence,
    DateTimeOffset MeasuredAt,
    double SignalStrengthDbm);
