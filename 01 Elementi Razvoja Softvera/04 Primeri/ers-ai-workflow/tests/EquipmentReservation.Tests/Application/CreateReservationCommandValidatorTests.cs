using EquipmentReservation.Application.Reservations.Create;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Application;

public sealed class CreateReservationCommandValidatorTests
{
    private readonly CreateReservationCommandValidator _validator = new();

    [Test]
    public void Validate_WhenCommandIsComplete_Succeeds()
    {
        var result = _validator.Validate(ValidCommand());

        Assert.That(result.Success, Is.True);
    }

    [Test]
    public void Validate_WhenRequestIdIsMissing_ReturnsStableErrorCode()
    {
        var result = _validator.Validate(ValidCommand() with { RequestId = Guid.Empty });

        Assert.That(result.Error, Is.EqualTo(CreateReservationErrorCodes.RequestIdRequired));
    }

    [Test]
    public void Validate_WhenEquipmentIdIsMissing_ReturnsStableErrorCode()
    {
        var result = _validator.Validate(ValidCommand() with { EquipmentId = Guid.Empty });

        Assert.That(result.Error, Is.EqualTo(CreateReservationErrorCodes.EquipmentIdRequired));
    }

    [Test]
    public void Validate_WhenStudentIdIsMissing_ReturnsStableErrorCode()
    {
        var result = _validator.Validate(ValidCommand() with { StudentId = Guid.Empty });

        Assert.That(result.Error, Is.EqualTo(CreateReservationErrorCodes.StudentIdRequired));
    }

    [TestCase(0)]
    [TestCase(-3)]
    public void Validate_WhenQuantityIsNotPositive_ReturnsStableErrorCode(int quantity)
    {
        var result = _validator.Validate(ValidCommand() with { Quantity = quantity });

        Assert.That(result.Error, Is.EqualTo(CreateReservationErrorCodes.QuantityMustBePositive));
    }

    private static CreateReservationCommand ValidCommand() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Quantity: 1);
}
