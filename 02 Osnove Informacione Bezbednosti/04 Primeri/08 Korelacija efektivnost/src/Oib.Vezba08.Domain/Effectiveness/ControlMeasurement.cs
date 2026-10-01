namespace Oib.Vezba08.Domain.Effectiveness;

public sealed record ControlMeasurement(
    string Control,
    int Attempts,
    int BlockedAttempts);
