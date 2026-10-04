namespace Odp.Vezba08.Domain.Fencing;

public sealed class FencingPolicy
{
    /// <summary>Bivši vlasnik ne zna da je smenjen; zato resurs sam odbija stariji token.</summary>
    public string Decide(long token, long highestSeenToken) =>
        token >= highestSeenToken ? FencingCodes.WriteAccepted : FencingCodes.StaleToken;
}
