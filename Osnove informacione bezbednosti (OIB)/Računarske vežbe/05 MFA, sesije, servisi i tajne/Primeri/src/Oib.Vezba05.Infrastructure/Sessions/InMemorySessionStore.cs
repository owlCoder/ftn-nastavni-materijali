using Oib.Vezba05.Application.Ports;
using Oib.Vezba05.Domain.Sessions;

namespace Oib.Vezba05.Infrastructure.Sessions;

public sealed class InMemorySessionStore(IEnumerable<AuthSession> sessions) : ISessionStore
{
    private readonly IReadOnlyDictionary<string, AuthSession> _sessions =
        sessions.ToDictionary(session => session.Id, StringComparer.Ordinal);

    public AuthSession? FindById(string sessionId) => _sessions.GetValueOrDefault(sessionId);
}
