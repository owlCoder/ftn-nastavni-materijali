using Oib.Vezba04.Application.Assessment;
using Oib.Vezba04.Domain.Risk;
using Oib.Vezba04.Infrastructure.Modeling;

namespace Oib.Vezba04.ConsoleUi;

public static class CompositionRoot
{
    public static ThreatModelDemo CreateDemo(TextWriter output) =>
        new(
            new AssessThreatModelHandler(
                new InMemoryThreatScenarioRepository(DemoData.Scenarios),
                new ThreatScenarioValidator(),
                new ThreatRiskCalculator(RiskPolicy.Default)),
            output);
}
