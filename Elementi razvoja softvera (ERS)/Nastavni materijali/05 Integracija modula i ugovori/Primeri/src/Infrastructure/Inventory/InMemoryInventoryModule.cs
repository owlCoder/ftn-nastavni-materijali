using EquipmentReservation.Application.Inventory.Models;
using System.Collections.Concurrent;
using EquipmentReservation.Application.Interfaces.Inventory;
using EquipmentReservation.Domain.Inventory;
using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Infrastructure.Inventory;

public sealed class InMemoryInventoryModule : IInventoryModule, IInventoryReadModel
{
    private readonly ConcurrentDictionary<Guid, InventorySlot> _slots = new();
    private readonly InventoryReservationService _reservationService;
    private readonly List<string> _initializationErrors = new();

    public IReadOnlyList<string> InitializationErrors => _initializationErrors;

    public InMemoryInventoryModule(InventoryReservationService reservationService, IEnumerable<InventoryItem> initialItems)
    {
        _reservationService = reservationService;

        if (initialItems is null)
        {
            _initializationErrors.Add("InitialInventoryMissing");
            return;
        }

        foreach (var item in initialItems)
        {
            if (item is null || item.EquipmentId == Guid.Empty || item.Available < 0)
            {
                _initializationErrors.Add("InvalidInitialInventoryItem");
                continue;
            }

            if (!_slots.TryAdd(item.EquipmentId, new InventorySlot(item)))
                _initializationErrors.Add($"DuplicateEquipmentId:{item.EquipmentId}");
        }
    }

    public Task<Result> ReserveAsync(ReserveInventoryRequest request, CancellationToken cancellationToken)
    {
        if (request is null || request.ReservationId == Guid.Empty)
            return Task.FromResult(Result.Fail(InventoryErrorCodes.InvalidEquipment));
        if (cancellationToken.IsCancellationRequested)
            return Task.FromResult(Result.Fail(InventoryErrorCodes.RequestCancelled));
        if (!_slots.TryGetValue(request.EquipmentId, out var slot))
            return Task.FromResult(Result.Fail(InventoryErrorCodes.EquipmentNotFound));

        lock (slot.SyncRoot)
        {
            if (slot.Reservations.ContainsKey(request.ReservationId))
                return Task.FromResult(Result.Fail(InventoryErrorCodes.DuplicateReservation));

            var result = _reservationService.Reserve(slot.Item, request.Quantity);
            if (!result.Success)
                return Task.FromResult(Result.Fail(result.Error));

            slot.Item = result.Value;
            slot.Reservations.Add(request.ReservationId, request.Quantity);
            return Task.FromResult(Result.Ok());
        }
    }

    public Task<Result> ReleaseAsync(ReserveInventoryRequest request, CancellationToken cancellationToken)
    {
        if (request is null || request.ReservationId == Guid.Empty || request.Quantity <= 0 || !_slots.TryGetValue(request.EquipmentId, out var slot))
            return Task.FromResult(Result.Fail(InventoryErrorCodes.EquipmentNotFound));

        lock (slot.SyncRoot)
        {
            if (!slot.Reservations.TryGetValue(request.ReservationId, out var quantity) || quantity != request.Quantity)
                return Task.FromResult(Result.Fail(InventoryErrorCodes.DuplicateReservation));

            if (slot.Item.Available + quantity > slot.Capacity)
                return Task.FromResult(Result.Fail(InventoryErrorCodes.InvalidRelease));

            slot.Reservations.Remove(request.ReservationId);
            slot.Item = new InventoryItem(slot.Item.EquipmentId, slot.Item.Available + quantity);
            return Task.FromResult(Result.Ok());
        }
    }

    public Task<EquipmentAvailability> GetAvailableAsync(Guid equipmentId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested || !_slots.TryGetValue(equipmentId, out var slot))
            return Task.FromResult(new EquipmentAvailability());

        lock (slot.SyncRoot)
            return Task.FromResult(new EquipmentAvailability(equipmentId, slot.Item.Available));
    }
}
