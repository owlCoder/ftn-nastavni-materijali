using Oib.Vezba01.Domain.Authorization;

namespace Oib.Vezba01.Application.Ports;

public interface IRoleCatalog
{
    Role? FindByName(string roleName);
}
