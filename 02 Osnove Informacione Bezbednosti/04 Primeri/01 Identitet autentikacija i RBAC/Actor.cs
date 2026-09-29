namespace Oib.Vezba01;

public sealed record Actor(string Id, bool IsAuthenticated, IReadOnlySet<string> Roles);

