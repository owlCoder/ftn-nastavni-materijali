using Odp.Vezba06.Application.Ports;
using Odp.Vezba06.Domain.Commands;

namespace Odp.Vezba06.Infrastructure.Commands;

public sealed class InMemoryCommandStore : ICommandStore
{
    private readonly Dictionary<string, DeviceCommand> _commands = new(StringComparer.Ordinal);

    public DeviceCommand? Find(string commandId) => _commands.GetValueOrDefault(commandId);

    public IReadOnlyList<DeviceCommand> All() =>
        _commands.Values.OrderBy(command => command.CommandId).ToArray();

    public void Save(DeviceCommand command) => _commands[command.CommandId] = command;
}
