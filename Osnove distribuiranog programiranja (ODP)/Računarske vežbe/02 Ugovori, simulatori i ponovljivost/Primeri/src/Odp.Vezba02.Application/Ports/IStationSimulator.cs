using Odp.Vezba02.Domain.Contracts;

namespace Odp.Vezba02.Application.Ports;

public interface IStationSimulator
{
    IReadOnlyList<TelemetryMessage> Emit(int count);
}
