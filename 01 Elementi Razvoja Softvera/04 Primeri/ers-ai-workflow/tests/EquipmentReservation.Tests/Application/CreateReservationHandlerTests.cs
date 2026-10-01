using EquipmentReservation.Application.Ports.Inventory;
using EquipmentReservation.Application.Ports.Reservations;
using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.Domain.Inventory;
using EquipmentReservation.Domain.Reservations;
using EquipmentReservation.Domain.Shared;
using Moq;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Application;

public sealed class CreateReservationHandlerTests
{
    private static readonly Guid GeneratedReservationId =
        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    private Mock<IReservationRepository> _reservations = null!;
    private Mock<IInventoryModule> _inventory = null!;
    private CreateReservationHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _reservations = new Mock<IReservationRepository>();
        _inventory = new Mock<IInventoryModule>();

        var requestLock = new Mock<IReservationRequestLock>();
        requestLock
            .Setup(port => port.AcquireAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IAsyncDisposable>());

        var idGenerator = new Mock<IReservationIdGenerator>();
        idGenerator.Setup(port => port.NewReservationId()).Returns(GeneratedReservationId);

        _handler = new CreateReservationHandler(
            new CreateReservationCommandValidator(),
            _reservations.Object,
            _inventory.Object,
            requestLock.Object,
            idGenerator.Object);
    }

    [Test]
    public async Task Handle_WhenInventoryAccepts_StoresConfirmedReservation()
    {
        var command = ValidCommand();
        InventoryReturns(Result.Ok());

        var result = await _handler.HandleAsync(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(CreateReservationOutcome.Confirmed));
            Assert.That(result.ReservationId, Is.EqualTo(GeneratedReservationId));
            Assert.That(result.ErrorCode, Is.Null);
            Assert.That(result.Replayed, Is.False);
        }
        _reservations.Verify(
            port => port.AddAsync(
                It.Is<Reservation>(stored =>
                    stored.Id == GeneratedReservationId &&
                    stored.RequestId == command.RequestId &&
                    stored.Status == ReservationStatus.Confirmed),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_WhenInventoryRejects_StoresRejectionWithInventoryErrorCode()
    {
        InventoryReturns(Result.Fail(InventoryErrorCodes.InsufficientStock));

        var result = await _handler.HandleAsync(ValidCommand(), CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(CreateReservationOutcome.Rejected));
            Assert.That(result.ErrorCode, Is.EqualTo(InventoryErrorCodes.InsufficientStock));
        }
        _reservations.Verify(
            port => port.AddAsync(
                It.Is<Reservation>(stored => stored.Status == ReservationStatus.Rejected),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_PassesGeneratedReservationIdThroughTheInventoryContract()
    {
        var command = ValidCommand();
        InventoryReturns(Result.Ok());

        await _handler.HandleAsync(command, CancellationToken.None);

        _inventory.Verify(
            port => port.ReserveAsync(
                new ReserveInventoryRequest(
                    command.EquipmentId,
                    command.Quantity,
                    GeneratedReservationId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_WhenCommandIsInvalid_ReturnsInvalidWithoutCallingPorts()
    {
        var command = ValidCommand() with { Quantity = 0 };

        var result = await _handler.HandleAsync(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(CreateReservationOutcome.Invalid));
            Assert.That(
                result.ErrorCode,
                Is.EqualTo(CreateReservationErrorCodes.QuantityMustBePositive));
            Assert.That(result.ReservationId, Is.Null);
        }
        _inventory.VerifyNoOtherCalls();
        _reservations.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Handle_WhenRequestWasAlreadyProcessed_ReplaysWithoutReservingAgain()
    {
        var command = ValidCommand();
        var existing = Reservation.Confirmed(
            Guid.NewGuid(),
            command.RequestId,
            command.EquipmentId,
            command.StudentId,
            command.Quantity);
        _reservations
            .Setup(port => port.FindByRequestIdAsync(
                command.RequestId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var result = await _handler.HandleAsync(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.ReservationId, Is.EqualTo(existing.Id));
            Assert.That(result.Replayed, Is.True);
        }
        _inventory.VerifyNoOtherCalls();
        _reservations.Verify(
            port => port.AddAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private void InventoryReturns(Result result) =>
        _inventory
            .Setup(port => port.ReserveAsync(
                It.IsAny<ReserveInventoryRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

    private static CreateReservationCommand ValidCommand() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Quantity: 2);
}
