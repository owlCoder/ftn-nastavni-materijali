using Oib.Vezba08.Domain.Shared;
namespace Oib.Vezba08.Domain.Effectiveness;

public sealed class ControlEffectivenessCalculator(decimal targetRatio) : IControlEffectivenessCalculator
{
    public Result<ControlEffectiveness> Evaluate(ControlMeasurement? measurement)
    {
        if (targetRatio < 0m || targetRatio > 1m)
            return Result<ControlEffectiveness>.Fail("InvalidTargetRatio");
        if (measurement is null || string.IsNullOrWhiteSpace(measurement.Control) ||
            measurement.Attempts < 0 || measurement.BlockedAttempts < 0 ||
            measurement.BlockedAttempts > measurement.Attempts)
            return Result<ControlEffectiveness>.Fail("InvalidMeasurement");
        if (measurement.Attempts == 0)
            return Result<ControlEffectiveness>.Ok(new(measurement.Control, 0m, false));
        var ratio = decimal.Divide(measurement.BlockedAttempts, measurement.Attempts);
        return Result<ControlEffectiveness>.Ok(new(measurement.Control, ratio, ratio >= targetRatio));
    }
}
