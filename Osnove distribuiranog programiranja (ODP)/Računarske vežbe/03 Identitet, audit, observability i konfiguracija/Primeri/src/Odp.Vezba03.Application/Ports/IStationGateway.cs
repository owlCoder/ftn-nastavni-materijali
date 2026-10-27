using Odp.Vezba03.Domain.Contacts;

namespace Odp.Vezba03.Application.Ports;

public interface IStationGateway
{
    /// <summary>Poziv druge komponente; identifikator operacije putuje zajedno sa zahtevom.</summary>
    bool Reserve(ContactRequest request, string correlationId);
}
