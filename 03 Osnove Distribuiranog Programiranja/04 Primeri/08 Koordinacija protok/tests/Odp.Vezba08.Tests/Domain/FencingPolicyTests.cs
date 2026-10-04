using NUnit.Framework;
using Odp.Vezba08.Domain.Fencing;

namespace Odp.Vezba08.Tests.Domain;

public sealed class FencingPolicyTests
{
    private readonly FencingPolicy _policy = new();

    [TestCase(2, 1, FencingCodes.WriteAccepted)]
    [TestCase(2, 2, FencingCodes.WriteAccepted)]
    [TestCase(1, 2, FencingCodes.StaleToken)]
    public void Decide_RejectsOnlyTokensOlderThanTheHighestSeen(long token, long highestSeen, string code)
    {
        Assert.That(_policy.Decide(token, highestSeen), Is.EqualTo(code));
    }
}
