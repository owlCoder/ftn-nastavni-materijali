using Oib.Vezba01.Application.Ports;
using Oib.Vezba01.Domain.Authorization;

namespace Oib.Vezba01.Infrastructure.Authorization;

public sealed class InMemoryRoleCatalog(IEnumerable<Role> roles) : IRoleCatalog
{
    private readonly IReadOnlyDictionary<string, Role> _roles =
        roles.ToDictionary(role => role.Name, StringComparer.OrdinalIgnoreCase);

    public Role? FindByName(string roleName) => _roles.GetValueOrDefault(roleName);
}
