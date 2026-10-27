namespace Odp.Vezba08.Domain.Leases;

public sealed record LeaseDecision(bool Granted, string Code, Lease Lease);
