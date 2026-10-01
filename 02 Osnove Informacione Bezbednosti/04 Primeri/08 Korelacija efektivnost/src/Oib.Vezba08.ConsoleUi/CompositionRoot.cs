using Oib.Vezba08.Application.Analysis;
using Oib.Vezba08.Domain.Correlation;
using Oib.Vezba08.Domain.Effectiveness;
using Oib.Vezba08.Infrastructure.Events;

namespace Oib.Vezba08.ConsoleUi;

public static class CompositionRoot
{
    public static IAnalyzeSecurityEventsUseCase CreateUseCase() =>
        new AnalyzeSecurityEventsHandler(
            new InMemorySecurityEventSource(DemoData.Events),
            new EventCorrelator(),
            new ControlEffectivenessCalculator(DemoData.TargetRatio));
}
