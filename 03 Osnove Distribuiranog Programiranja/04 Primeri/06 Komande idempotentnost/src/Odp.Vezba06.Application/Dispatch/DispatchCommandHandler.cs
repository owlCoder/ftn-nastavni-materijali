using Odp.Vezba06.Application.Ports;
using Odp.Vezba06.Domain.Commands;

namespace Odp.Vezba06.Application.Dispatch;

public sealed class DispatchCommandHandler(
    ICommandStore commands,
    IDeviceLink deviceLink,
    CommandLifecycle lifecycle,
    IClock clock) : IDispatchCommandUseCase
{
    public DispatchResult Dispatch(string commandId, string deviceId, string action)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandId);
        ArgumentException.ThrowIfNullOrWhiteSpace(deviceId);
        ArgumentException.ThrowIfNullOrWhiteSpace(action);

        var existing = commands.Find(commandId);
        if (existing is not null)
            return new(existing, CommandCodes.AlreadyDispatched);

        var command = lifecycle.Create(commandId, deviceId, action, clock.UtcNow);
        commands.Save(command);
        deviceLink.Send(command);

        return new(command, CommandCodes.Sent);
    }
}
