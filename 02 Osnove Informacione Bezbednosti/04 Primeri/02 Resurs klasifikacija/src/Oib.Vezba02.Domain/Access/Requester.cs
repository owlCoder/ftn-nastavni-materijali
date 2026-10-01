namespace Oib.Vezba02.Domain.Access;

public sealed record Requester(
    string ActorId,
    IReadOnlySet<string> Permissions);
