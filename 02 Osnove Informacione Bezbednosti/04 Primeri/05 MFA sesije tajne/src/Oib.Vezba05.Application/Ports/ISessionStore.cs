using Oib.Vezba05.Domain.Sessions;

namespace Oib.Vezba05.Application.Ports;

public interface ISessionStore
{
    AuthSession? FindById(string sessionId);
}
