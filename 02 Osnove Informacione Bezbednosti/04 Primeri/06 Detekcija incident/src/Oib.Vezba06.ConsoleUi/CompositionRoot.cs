using Oib.Vezba06.Application.Detection;
using Oib.Vezba06.Domain.Detection;
using Oib.Vezba06.Domain.Incidents;
using Oib.Vezba06.Infrastructure.Incidents;
using Oib.Vezba06.Infrastructure.Signals;
using Oib.Vezba06.Infrastructure.Time;

namespace Oib.Vezba06.ConsoleUi;

public static class CompositionRoot
{
    public static IDetectIncidentsUseCase CreateUseCase()
    {
        var clock = new SystemClock();

        return new DetectIncidentsHandler(
            new InMemoryLoginAttemptSource(DemoData.AttemptsAt(clock.UtcNow)),
            [new RepeatedFailedLoginRule(DemoData.DetectionThreshold, DemoData.DetectionWindow)],
            new IncidentFactory(new IncidentSeverityPolicy(DemoData.HighSeverityFrom)),
            new SequentialIncidentIdGenerator(),
            new InMemoryIncidentRepository(),
            clock);
    }
}
