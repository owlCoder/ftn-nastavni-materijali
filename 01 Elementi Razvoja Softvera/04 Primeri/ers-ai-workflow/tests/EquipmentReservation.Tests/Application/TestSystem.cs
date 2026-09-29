using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.Infrastructure.Inventory;
using EquipmentReservation.Infrastructure.Persistence;

namespace EquipmentReservation.Tests.Application;

internal sealed record TestSystem(
    CreateReservationHandler Handler,
    InMemoryInventoryModule Inventory,
    InMemoryReservationRepository Repository,
    Guid EquipmentId);
