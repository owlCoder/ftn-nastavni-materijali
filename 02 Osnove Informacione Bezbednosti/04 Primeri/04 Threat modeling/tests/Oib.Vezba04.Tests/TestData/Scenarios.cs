using Oib.Vezba04.Domain.Modeling;

namespace Oib.Vezba04.Tests.TestData;

internal static class Scenarios
{
    public static ThreatScenario Create(
        int likelihood,
        int businessImpact,
        bool crossesTrustBoundary,
        string name = "Scenario") =>
        new(
            name,
            new DataFlow("Tok", new Asset("Imovina", businessImpact), crossesTrustBoundary),
            likelihood,
            "Kontrola");
}
