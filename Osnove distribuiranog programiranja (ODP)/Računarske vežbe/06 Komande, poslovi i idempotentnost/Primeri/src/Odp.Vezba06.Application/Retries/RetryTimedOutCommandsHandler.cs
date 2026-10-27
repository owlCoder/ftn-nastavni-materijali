using Odp.Vezba06.Application.Ports;
using Odp.Vezba06.Domain.Commands;

namespace Odp.Vezba06.Application.Retries;

public sealed class RetryTimedOutCommandsHandler(
    ICommandStore commands,
    IDeviceLink deviceLink,
    CommandLifecycle lifecycle,
    IClock clock) : IRetryTimedOutCommandsUseCase
{
    public IReadOnlyList<RetryOutcome> Run()
    {
        var now = clock.UtcNow;

        return commands.All()
            .Where(command => lifecycle.HasTimedOut(command, now))
            .Select(command => RetryOrGiveUp(command, now))
            .ToArray();
    }

    private RetryOutcome RetryOrGiveUp(DeviceCommand command, DateTimeOffset now)
    {
        if (!lifecycle.CanRetry(command))
        {
            commands.Save(lifecycle.GiveUp(command));
            return new(command.CommandId, CommandCodes.AttemptsExhausted);
        }

        var retried = lifecycle.Retry(command, now);
        commands.Save(retried);
        deviceLink.Send(retried);

        return new(command.CommandId, CommandCodes.Retried);
    }
}
