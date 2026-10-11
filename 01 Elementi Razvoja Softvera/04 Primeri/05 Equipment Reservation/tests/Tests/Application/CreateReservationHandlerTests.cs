using EquipmentReservation.Application.Inventory.Models;
using EquipmentReservation.Application.Interfaces.Inventory;
using EquipmentReservation.Application.Interfaces.Reservations;
using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.Application.Results;
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
        _reservations.Setup(port => port.FindByRequestIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Reservation());
        _reservations.Setup(port => port.AddAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var requestLock = new Mock<IReservationRequestLock>();
        requestLock
            .Setup(port => port.AcquireAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<ReservationLockAcquisition>(ReservationLockAcquisition.Success(Mock.Of<IAsyncDisposable>())));

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
            Assert.That(result.ErrorCode, Is.Empty);
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
                It.Is<ReserveInventoryRequest>(request =>
                    request.EquipmentId == command.EquipmentId &&
                    request.Quantity == command.Quantity &&
                    request.ReservationId == GeneratedReservationId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_WhenCommandIsInvalid_ReturnsInvalidWithoutCallingPorts()
    {
        var command = new CreateReservationCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0);

        var result = await _handler.HandleAsync(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(CreateReservationOutcome.Invalid));
            Assert.That(
                result.ErrorCode,
                Is.EqualTo(CreateReservationErrorCodes.QuantityMustBePositive));
            Assert.That(result.ReservationId, Is.EqualTo(Guid.Empty));
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

    [Test]
    public async Task Handle_WhenPersistenceFails_CompensatesReservedStock()
    {
        InventoryReturns(Result.Ok());
        _reservations.Setup(port => port.AddAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _inventory.Setup(port => port.ReleaseAsync(It.IsAny<ReserveInventoryRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        var result = await _handler.HandleAsync(ValidCommand(), CancellationToken.None);

        Assert.That(result.ErrorCode, Is.EqualTo(CreateReservationErrorCodes.PersistenceFailed));
        _inventory.Verify(port => port.ReleaseAsync(It.IsAny<ReserveInventoryRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_WhenCompensationFails_ReturnsSeparateErrorCode()
    {
        InventoryReturns(Result.Ok());
        _reservations.Setup(port => port.AddAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _inventory.Setup(port => port.ReleaseAsync(It.IsAny<ReserveInventoryRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(InventoryErrorCodes.DuplicateReservation));

        var result = await _handler.HandleAsync(ValidCommand(), CancellationToken.None);

        Assert.That(result.ErrorCode, Is.EqualTo(CreateReservationErrorCodes.CompensationFailed));
        _inventory.Verify(port => port.ReleaseAsync(It.IsAny<ReserveInventoryRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private void InventoryReturns(Result result) =>
        _inventory
            .Setup(port => port.ReserveAsync(
                It.IsAny<ReserveInventoryRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

    private static CreateReservationCommand ValidCommand() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), quantity: 2);
}
