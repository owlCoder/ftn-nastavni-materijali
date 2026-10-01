using EquipmentReservation.Application.Inventory.GetAvailability;
using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.ConsoleUi.Actions;
using EquipmentReservation.ConsoleUi.Menu;
using EquipmentReservation.ConsoleUi.Terminal;
using EquipmentReservation.Domain.Inventory;
using EquipmentReservation.Infrastructure.Concurrency;
using EquipmentReservation.Infrastructure.Identity;
using EquipmentReservation.Infrastructure.Inventory;
using EquipmentReservation.Infrastructure.Persistence;

namespace EquipmentReservation.ConsoleUi.Composition;

public static class CompositionRoot
{
    public static ConsoleMenu CreateMenu(ITerminal terminal)
    {
        var inventory = new InMemoryInventoryModule(
            new InventoryReservationService(),
            DemoInventory.Items);

        ICreateReservationUseCase createReservation = new CreateReservationHandler(
            new CreateReservationCommandValidator(),
            new InMemoryReservationRepository(),
            inventory,
            new InMemoryReservationRequestLock(),
            new GuidReservationIdGenerator());
        IGetEquipmentAvailabilityUseCase getAvailability =
            new GetEquipmentAvailabilityHandler(inventory);

        return new ConsoleMenu(
            terminal,
            [
                new ShowAvailabilityAction(getAvailability, terminal, DemoInventory.EquipmentId),
                new CreateReservationAction(
                    createReservation,
                    getAvailability,
                    terminal,
                    DemoInventory.EquipmentId)
            ]);
    }
}
