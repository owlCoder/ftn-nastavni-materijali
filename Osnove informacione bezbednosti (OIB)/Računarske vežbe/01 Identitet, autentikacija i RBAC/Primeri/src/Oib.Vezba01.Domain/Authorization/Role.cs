namespace Oib.Vezba01.Domain.Authorization;

public sealed record Role(
    string Name,
    IReadOnlySet<string> Permissions);
