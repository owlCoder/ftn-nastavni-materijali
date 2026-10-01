namespace EquipmentReservation.Application.Ports.Reservations;

public interface IReservationIdGenerator
{
    Guid NewReservationId();
}
