namespace Oib.Vezba08;

public sealed record ControlMeasurement(string Control, int Attempts, int BlockedAttempts)
{
    public decimal Effectiveness => Attempts == 0 ? 0 : decimal.Divide(BlockedAttempts, Attempts);
}

