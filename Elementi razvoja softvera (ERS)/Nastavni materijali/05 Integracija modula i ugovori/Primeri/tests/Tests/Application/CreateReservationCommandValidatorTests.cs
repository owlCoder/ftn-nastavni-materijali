using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.Domain.Reservations;
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
        var result = _validator.Validate(new CreateReservationCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), 1));

        Assert.That(result.Error, Is.EqualTo(CreateReservationErrorCodes.RequestIdRequired));
    }

    [Test]
    public void Validate_WhenEquipmentIdIsMissing_ReturnsStableErrorCode()
    {
        var result = _validator.Validate(new CreateReservationCommand(Guid.NewGuid(), Guid.Empty, Guid.NewGuid(), 1));

        Assert.That(result.Error, Is.EqualTo(CreateReservationErrorCodes.EquipmentIdRequired));
    }

    [Test]
    public void Validate_WhenStudentIdIsMissing_ReturnsStableErrorCode()
    {
        var result = _validator.Validate(new CreateReservationCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, 1));

        Assert.That(result.Error, Is.EqualTo(CreateReservationErrorCodes.StudentIdRequired));
    }

    [TestCase(0)]
    [TestCase(-3)]
    public void Validate_WhenQuantityIsNotPositive_ReturnsStableErrorCode(int quantity)
    {
        var result = _validator.Validate(new CreateReservationCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), quantity));

        Assert.That(result.Error, Is.EqualTo(CreateReservationErrorCodes.QuantityMustBePositive));
    }

    private static CreateReservationCommand ValidCommand() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), quantity: 1);
}
