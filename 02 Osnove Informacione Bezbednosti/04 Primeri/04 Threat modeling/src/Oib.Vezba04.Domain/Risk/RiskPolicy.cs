namespace Oib.Vezba04.Domain.Risk;

public sealed record RiskPolicy(
    int TrustBoundaryMultiplier,
    int TreatmentThreshold)
{
    public static RiskPolicy Default { get; } = new(
        TrustBoundaryMultiplier: 2,
        TreatmentThreshold: 24);
}
