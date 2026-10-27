using NUnit.Framework;
using Odp.Vezba03.Domain.Contacts;

namespace Odp.Vezba03.Tests.Domain;

public sealed class ContactRequestValidatorTests
{
    private static readonly ContactRequest Valid = new("M-ARGUS", "GS-NOVI-SAD", TimeSpan.FromMinutes(10));

    private readonly ContactRequestValidator _validator = new(new ContactLimits(TimeSpan.FromMinutes(15)));

    [Test]
    public void Validate_WhenRequestIsWithinLimits_Accepts()
    {
        Assert.That(_validator.Validate(Valid).Success, Is.True);
    }

    [Test]
    public void Validate_WhenDurationEqualsConfiguredLimit_Accepts()
    {
        var result = _validator.Validate(Valid with { Duration = TimeSpan.FromMinutes(15) });

        Assert.That(result.Success, Is.True);
    }

    [Test]
    public void Validate_WhenDurationExceedsConfiguredLimit_Rejects()
    {
        var result = _validator.Validate(Valid with { Duration = TimeSpan.FromMinutes(16) });

        Assert.That(result.Error, Is.EqualTo(ContactCodes.DurationTooLong));
    }

    [Test]
    public void Validate_WhenDurationIsNotPositive_Rejects()
    {
        var result = _validator.Validate(Valid with { Duration = TimeSpan.Zero });

        Assert.That(result.Error, Is.EqualTo(ContactCodes.DurationMustBePositive));
    }

    [Test]
    public void Validate_WhenStationIsMissing_Rejects()
    {
        var result = _validator.Validate(Valid with { StationId = "" });

        Assert.That(result.Error, Is.EqualTo(ContactCodes.StationIdRequired));
    }
}
