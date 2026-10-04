using Odp.Vezba06.Application.Ports;
using Odp.Vezba06.Domain.Commands;

namespace Odp.Vezba06.Application.Acknowledgement;

public sealed class AcknowledgeCommandHandler(ICommandStore commands, CommandLifecycle lifecycle)
    : IAcknowledgeCommandUseCase
{
    public string Acknowledge(string commandId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandId);

        var command = commands.Find(commandId);
        if (command is null)
            return CommandCodes.CommandUnknown;

        var (updated, code) = lifecycle.Acknowledge(command);
        commands.Save(updated);

        return code;
    }
}
