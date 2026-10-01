using Oib.Vezba05.Application.StepUp;
using Oib.Vezba05.Domain.StepUp;
using Oib.Vezba05.Infrastructure.Sessions;
using Oib.Vezba05.Infrastructure.Time;

namespace Oib.Vezba05.ConsoleUi;

public static class CompositionRoot
{
    public static StepUpDemo CreateDemo(TextWriter output)
    {
        var clock = new SystemClock();

        return new StepUpDemo(
            new AuthorizeRiskyOperationHandler(
                new InMemorySessionStore(DemoData.SessionsAt(clock.UtcNow)),
                new StepUpAuthenticationPolicy(),
                clock),
            output);
    }
}
