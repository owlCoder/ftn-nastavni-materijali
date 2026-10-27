using Oib.Vezba06.Application.Ports;
using Oib.Vezba06.Domain.Signals;

namespace Oib.Vezba06.Infrastructure.Signals;

public sealed class InMemoryLoginAttemptSource(IEnumerable<LoginAttempt> attempts)
    : ILoginAttemptSource
{
    private readonly IReadOnlyCollection<LoginAttempt> _attempts = attempts.ToArray();

    public IReadOnlyCollection<LoginAttempt> GetAttempts() => _attempts;
}
