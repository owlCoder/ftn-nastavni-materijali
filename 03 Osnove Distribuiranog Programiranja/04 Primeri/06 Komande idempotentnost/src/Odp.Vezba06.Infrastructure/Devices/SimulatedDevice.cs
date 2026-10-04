using Odp.Vezba06.Application.Ports;
using Odp.Vezba06.Domain.Commands;

namespace Odp.Vezba06.Infrastructure.Devices;

/// <summary>Uređaj koji isti CommandId izvršava jednom, koliko god puta da ga primi.</summary>
public sealed class SimulatedDevice : IDeviceLink
{
    private readonly HashSet<string> _executed = new(StringComparer.Ordinal);

    public int Deliveries { get; private set; }

    public IReadOnlyCollection<string> ExecutedCommandIds => _executed;

    public void Send(DeviceCommand command)
    {
        Deliveries++;
        _executed.Add(command.CommandId);
    }
}
