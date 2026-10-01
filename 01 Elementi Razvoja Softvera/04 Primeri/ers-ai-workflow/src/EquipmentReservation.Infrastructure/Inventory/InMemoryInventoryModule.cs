using System.Collections.Concurrent;
using EquipmentReservation.Application.Ports.Inventory;
using EquipmentReservation.Domain.Inventory;
using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Infrastructure.Inventory;

public sealed class InMemoryInventoryModule : IInventoryModule, IInventoryReadModel
{
    private readonly ConcurrentDictionary<Guid, InventorySlot> _slots = new();
    private readonly InventoryReservationService _reservationService;

    public InMemoryInventoryModule(
        InventoryReservationService reservationService,
        IEnumerable<InventoryItem> initialItems)
    {
        _reservationService = reservationService
            ?? throw new ArgumentNullException(nameof(reservationService));
        ArgumentNullException.ThrowIfNull(initialItems);

        foreach (var item in initialItems)
        {
            if (!_slots.TryAdd(item.EquipmentId, new InventorySlot(item)))
                throw new ArgumentException(
                    $"Equipment '{item.EquipmentId}' is listed more than once.",
                    nameof(initialItems));
        }
    }

    public Task<Result> ReserveAsync(
        ReserveInventoryRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();

        if (!_slots.TryGetValue(request.EquipmentId, out var slot))
            return Task.FromResult(Result.Fail(InventoryErrorCodes.EquipmentNotFound));

        lock (slot.SyncRoot)
        {
            var reserved = _reservationService.Reserve(slot.Item, request.Quantity);
            if (!reserved.Success)
                return Task.FromResult(Result.Fail(reserved.Error));

            slot.Item = reserved.Value;
            return Task.FromResult(Result.Ok());
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
            return Task.FromResult<int?>(slot.Item.Available);
        }
    }
}
