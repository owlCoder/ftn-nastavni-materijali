namespace EquipmentReservation.Application.Interfaces.Reservations;

public interface IReservationIdGenerator
{
    Guid NewReservationId();
}
