namespace Oib.Vezba02;

public sealed record AccessRequest(string ActorId, string Action, IReadOnlySet<string> Permissions);

