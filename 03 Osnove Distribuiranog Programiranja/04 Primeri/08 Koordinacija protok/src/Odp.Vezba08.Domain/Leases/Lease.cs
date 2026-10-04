namespace Odp.Vezba08.Domain.Leases;

/// <summary>Privremeno vlasništvo nad resursom; token raste pri svakoj promeni vlasnika.</summary>
public sealed record Lease(string Resource, string OwnerId, long FencingToken, DateTimeOffset ExpiresAt);
