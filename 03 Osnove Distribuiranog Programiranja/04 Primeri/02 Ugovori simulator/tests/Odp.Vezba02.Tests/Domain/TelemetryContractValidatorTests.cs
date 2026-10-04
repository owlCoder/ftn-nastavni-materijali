using NUnit.Framework;
using Odp.Vezba02.Domain.Contracts;

namespace Odp.Vezba02.Tests.Domain;

public sealed class TelemetryContractValidatorTests
{
    private static readonly TelemetryMessage Valid = new(
        TelemetryContract.Version,
        "GS-NOVI-SAD",
        1,
        new DateTimeOffset(2027, 2, 8, 9, 0, 0, TimeSpan.Zero),
        -92);

    private readonly TelemetryContractValidator _validator = new();

    [Test]
    public void Validate_WhenMessageFollowsContract_Accepts()
    {
        Assert.That(_validator.Validate(Valid).Success, Is.True);
    }

    [TestCase(-130)]
    [TestCase(-20)]
    public void Validate_WhenSignalIsOnContractBoundary_Accepts(double signal)
    {
        Assert.That(_validator.Validate(Valid with { SignalStrengthDbm = signal }).Success, Is.True);
    }

    [Test]
    public void Validate_WhenVersionIsNotSupported_Rejects()
    {
        var result = _validator.Validate(Valid with { ContractVersion = "2.0" });

        Assert.That(result.Error, Is.EqualTo(ContractErrorCodes.UnsupportedVersion));
    }

    [Test]
    public void Validate_WhenStationIdIsMissing_Rejects()
    {
        var result = _validator.Validate(Valid with { StationId = " " });

        Assert.That(result.Error, Is.EqualTo(ContractErrorCodes.StationIdRequired));
    }

    [Test]
    public void Validate_WhenSequenceIsNotPositive_Rejects()
    {
        var result = _validator.Validate(Valid with { Sequence = 0 });

        Assert.That(result.Error, Is.EqualTo(ContractErrorCodes.SequenceMustBePositive));
    }

    [TestCase(-130.5)]
    [TestCase(5)]
    public void Validate_WhenSignalIsOutsideContractRange_Rejects(double signal)
    {
        var result = _validator.Validate(Valid with { SignalStrengthDbm = signal });

        Assert.That(result.Error, Is.EqualTo(ContractErrorCodes.SignalOutOfRange));
    }
}
