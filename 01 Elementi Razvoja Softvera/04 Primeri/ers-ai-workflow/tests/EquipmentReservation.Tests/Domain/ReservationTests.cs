using EquipmentReservation.Domain.Reservations;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Domain;

public sealed class ReservationTests
{
    [Test]
    public void Confirmed_CreatesReservationWithoutRejectionReason()
    {
        var reservation = Reservation.Confirmed(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            quantity: 2);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(reservation.Status, Is.EqualTo(ReservationStatus.Confirmed));
            Assert.That(reservation.RejectionReason, Is.Null);
        }
    }

    [Test]
    public void Rejected_KeepsTheReasonOfRejection()
    {
        var reservation = Reservation.Rejected(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            quantity: 2,
            rejectionReason: "InsufficientStock");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(reservation.Status, Is.EqualTo(ReservationStatus.Rejected));
            Assert.That(reservation.RejectionReason, Is.EqualTo("InsufficientStock"));
        }
    }

    [Test]
    public void Rejected_WithoutReason_IsNotAValidState()
    {
        Action createWithoutReason = () => Reservation.Rejected(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            quantity: 2,
            rejectionReason: " ");

        Assert.That(createWithoutReason, Throws.ArgumentException);
    }
}
