namespace Oib.Vezba04.Domain.Modeling;

public sealed record ThreatScenario(
    string Name,
    DataFlow Flow,
    int Likelihood,
    string ProposedControl);
