using Oib.Vezba01.Domain.Identity;

namespace Oib.Vezba01.Domain.Authorization;

public interface IRbacPolicy
{
    AccessDecision Decide(Actor? actor, string? permission, IEnumerable<Role>? assignedRoles);
}
