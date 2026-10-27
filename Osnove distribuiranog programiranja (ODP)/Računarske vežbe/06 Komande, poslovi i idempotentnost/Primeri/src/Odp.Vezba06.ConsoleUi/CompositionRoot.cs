using Odp.Vezba06.Application.Acknowledgement;
using Odp.Vezba06.Application.Dispatch;
using Odp.Vezba06.Application.Retries;
using Odp.Vezba06.Domain.Commands;
using Odp.Vezba06.Infrastructure.Commands;
using Odp.Vezba06.Infrastructure.Devices;
using Odp.Vezba06.Infrastructure.Time;

namespace Odp.Vezba06.ConsoleUi;

public static class CompositionRoot
{
    public static CommandDemo CreateDemo(TextWriter output)
    {
        var clock = new ManualClock(DemoData.Start);
        var commands = new InMemoryCommandStore();
        var device = new SimulatedDevice();
        var lifecycle = new CommandLifecycle(DemoData.Rules);

        return new CommandDemo(
            new DispatchCommandHandler(commands, device, lifecycle, clock),
            new RetryTimedOutCommandsHandler(commands, device, lifecycle, clock),
            new AcknowledgeCommandHandler(commands, lifecycle),
            device,
            clock,
            output);
    }
}
