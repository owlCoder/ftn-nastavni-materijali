using EquipmentReservation.Domain.Inventory;

namespace EquipmentReservation.Infrastructure.Inventory;

public static class DemoInventory
{
    public static readonly Guid EquipmentId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static IReadOnlyList<InventoryItem> Items { get; } =
        [new InventoryItem(EquipmentId, Available: 10)];
}
