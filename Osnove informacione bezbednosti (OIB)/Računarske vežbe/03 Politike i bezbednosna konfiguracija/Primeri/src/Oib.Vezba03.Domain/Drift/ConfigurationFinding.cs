namespace Oib.Vezba03.Domain.Drift;

public sealed record ConfigurationFinding(
    string Control,
    string Expected,
    string Actual);
