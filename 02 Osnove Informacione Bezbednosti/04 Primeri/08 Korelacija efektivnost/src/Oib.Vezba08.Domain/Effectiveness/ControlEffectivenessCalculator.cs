namespace Oib.Vezba08.Domain.Effectiveness;

public sealed class ControlEffectivenessCalculator
{
    private readonly decimal _targetRatio;

    public ControlEffectivenessCalculator(decimal targetRatio)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(targetRatio, 0m);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(targetRatio, 1m);

        _targetRatio = targetRatio;
    }

    public ControlEffectiveness Evaluate(ControlMeasurement measurement)
    {
        ArgumentNullException.ThrowIfNull(measurement);
        ArgumentOutOfRangeException.ThrowIfNegative(measurement.Attempts);
        ArgumentOutOfRangeException.ThrowIfNegative(measurement.BlockedAttempts);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(
            measurement.BlockedAttempts,
            measurement.Attempts);

        if (measurement.Attempts == 0)
            return new(measurement.Control, 0m, MeetsTarget: false);

        var ratio = decimal.Divide(measurement.BlockedAttempts, measurement.Attempts);
        return new(measurement.Control, ratio, ratio >= _targetRatio);
    }
}
