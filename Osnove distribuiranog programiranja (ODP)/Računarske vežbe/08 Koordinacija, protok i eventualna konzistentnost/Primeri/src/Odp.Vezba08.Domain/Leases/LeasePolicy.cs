namespace Odp.Vezba08.Domain.Leases;

public sealed class LeasePolicy
{
    private readonly TimeSpan _duration;

    public LeasePolicy(TimeSpan duration)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(duration, TimeSpan.Zero);
        _duration = duration;
    }

    public LeaseDecision Acquire(Lease? current, string resource, string candidateId, DateTimeOffset now)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resource);
        ArgumentException.ThrowIfNullOrWhiteSpace(candidateId);

        if (current is null || now >= current.ExpiresAt)
        {
            var token = (current?.FencingToken ?? 0) + 1;
            return new(true, LeaseCodes.Granted, new Lease(resource, candidateId, token, now + _duration));
        }

        return current.OwnerId == candidateId
            ? new(true, LeaseCodes.Renewed, current with { ExpiresAt = now + _duration })
            : new(false, LeaseCodes.HeldByAnother, current);
    }
}
