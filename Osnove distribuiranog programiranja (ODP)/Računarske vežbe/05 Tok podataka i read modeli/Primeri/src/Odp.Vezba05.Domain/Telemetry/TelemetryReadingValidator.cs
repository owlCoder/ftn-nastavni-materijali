using Odp.Vezba05.Domain.Shared;

namespace Odp.Vezba05.Domain.Telemetry;

public sealed class TelemetryReadingValidator
{
    public Result Validate(TelemetryReading reading)
    {
        ArgumentNullException.ThrowIfNull(reading);

        if (string.IsNullOrWhiteSpace(reading.StationId))
            return Result.Fail(TelemetryCodes.StationIdRequired);
        if (reading.Sequence <= 0)
            return Result.Fail(TelemetryCodes.SequenceMustBePositive);

        return Result.Ok();
    }
}
