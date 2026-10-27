using Odp.Vezba06.Domain.Commands;

namespace Odp.Vezba06.Application.Ports;

public interface ICommandStore
{
    DeviceCommand? Find(string commandId);

    IReadOnlyList<DeviceCommand> All();

    void Save(DeviceCommand command);
}
