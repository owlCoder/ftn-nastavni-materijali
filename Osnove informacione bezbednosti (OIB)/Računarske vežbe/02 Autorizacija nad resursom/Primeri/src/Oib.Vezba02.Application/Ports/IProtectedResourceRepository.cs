using Oib.Vezba02.Domain.Resources;

namespace Oib.Vezba02.Application.Ports;

public interface IProtectedResourceRepository
{
    ProtectedResource? FindById(string resourceId);
}
