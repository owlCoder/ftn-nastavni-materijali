using NUnit.Framework;
using Odp.Vezba05.Domain.ReadModels;

namespace Odp.Vezba05.Tests.Domain;

public sealed class FreshnessPolicyTests
{
    private static readonly DateTimeOffset MeasuredAt = new(2027, 3, 1, 9, 0, 0, TimeSpan.Zero);
    private static readonly StationView View = new("GS-NOVI-SAD", 1, MeasuredAt, -91);

    private readonly FreshnessPolicy _policy = new(TimeSpan.FromSeconds(30));

    [TestCase(0, false)]
    [TestCase(30, false)]
    [TestCase(31, true)]
    public void IsStale_ComparesAgeOfTheViewWithTheLimit(int ageSeconds, bool stale)
    {
        Assert.That(_policy.IsStale(View, MeasuredAt.AddSeconds(ageSeconds)), Is.EqualTo(stale));
    }
}
