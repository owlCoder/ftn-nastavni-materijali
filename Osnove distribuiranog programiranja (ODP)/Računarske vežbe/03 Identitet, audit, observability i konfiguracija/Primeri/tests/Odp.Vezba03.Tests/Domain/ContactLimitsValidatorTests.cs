using NUnit.Framework;
using Odp.Vezba03.Domain.Configuration;
using Odp.Vezba03.Domain.Contacts;

namespace Odp.Vezba03.Tests.Domain;

public sealed class ContactLimitsValidatorTests
{
    private readonly ContactLimitsValidator _validator = new();

    [TestCase(1)]
    [TestCase(60)]
    public void Validate_WhenLimitIsInsideAllowedRange_Accepts(int minutes)
    {
        Assert.That(_validator.Validate(new ContactLimits(TimeSpan.FromMinutes(minutes))).Success, Is.True);
    }

    [Test]
    public void Validate_WhenLimitIsNotPositive_Rejects()
    {
        var result = _validator.Validate(new ContactLimits(TimeSpan.Zero));

        Assert.That(result.Error, Is.EqualTo(ConfigurationCodes.MaxDurationMustBePositive));
    }

    [Test]
    public void Validate_WhenLimitIsAboveHardLimit_Rejects()
    {
        var result = _validator.Validate(new ContactLimits(TimeSpan.FromMinutes(61)));

        Assert.That(result.Error, Is.EqualTo(ConfigurationCodes.MaxDurationAboveHardLimit));
    }
}
