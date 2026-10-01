using EquipmentReservation.Application.Ports.Reservations;

namespace EquipmentReservation.Infrastructure.Identity;

public sealed class GuidReservationIdGenerator : IReservationIdGenerator
{
    public Guid NewReservationId() => Guid.NewGuid();
}
