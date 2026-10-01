namespace Oib.Vezba01.Domain.Identity;

public sealed record Actor(
    string Id,
    bool IsAuthenticated,
    IReadOnlySet<string> Roles);
