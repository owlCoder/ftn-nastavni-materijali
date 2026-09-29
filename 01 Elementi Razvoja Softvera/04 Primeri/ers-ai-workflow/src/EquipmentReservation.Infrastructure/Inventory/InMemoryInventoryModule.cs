using System.Collections.Concurrent;
using EquipmentReservation.Application.Ports.Inventory;

namespace EquipmentReservation.Infrastructure.Inventory;

public sealed class InMemoryInventoryModule : IInventoryModule, IInventoryReadModel
{
    private readonly ConcurrentDictionary<Guid, InventorySlot> _slots = new();

    public void Seed(Guid equipmentId, int available)
    {
        if (equipmentId == Guid.Empty)
            throw new ArgumentException("Equipment id is required.", nameof(equipmentId));
        if (available < 0)
            throw new ArgumentOutOfRangeException(nameof(available));

        _slots[equipmentId] = new InventorySlot(available);
    }

    public Task<ReserveInventoryResult> ReserveAsync(
        ReserveInventoryRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!_slots.TryGetValue(request.EquipmentId, out var slot))
            return Task.FromResult(new ReserveInventoryResult(false, "EquipmentNotFound"));

        lock (slot.SyncRoot)
        {
            if (request.Quantity <= 0)
                return Task.FromResult(new ReserveInventoryResult(false, "InvalidQuantity"));

            if (slot.Available < request.Quantity)
                return Task.FromResult(new ReserveInventoryResult(false, "InsufficientStock"));

            slot.Available -= request.Quantity;
            return Task.FromResult(new ReserveInventoryResult(true, null));
        }
    }

    public Task<int?> GetAvailableAsync(
        Guid equipmentId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!_slots.TryGetValue(equipmentId, out var slot))
            return Task.FromResult<int?>(null);

        lock (slot.SyncRoot)
        {
            return Task.FromResult<int?>(slot.Available);
        }
    }
}
