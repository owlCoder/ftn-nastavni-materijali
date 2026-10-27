namespace Oib.Vezba08.Domain.Effectiveness;

public sealed record ControlEffectiveness(
    string Control,
    decimal Ratio,
    bool MeetsTarget);
