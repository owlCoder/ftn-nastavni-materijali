using Odp.Vezba06.Domain.Commands;

namespace Odp.Vezba06.Application.Ports;

/// <summary>Slanje ka uređaju; povratna vrednost ne postoji jer potvrda stiže odvojeno ili nikad.</summary>
public interface IDeviceLink
{
    void Send(DeviceCommand command);
}
