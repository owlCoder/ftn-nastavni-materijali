using Oib.Vezba06.Domain.Signals;

namespace Oib.Vezba06.Application.Ports;

public interface ILoginAttemptSource
{
    IReadOnlyCollection<LoginAttempt> GetAttempts();
}
