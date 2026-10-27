using System.Collections.Concurrent;
using EquipmentReservation.Application.Interfaces.Reservations;
using EquipmentReservation.Domain.Reservations;

namespace EquipmentReservation.Infrastructure.Persistence;

public sealed class InMemoryReservationRepository : IReservationRepository
{
    private readonly ConcurrentDictionary<Guid, Reservation> _byRequestId = new();

    public Task<Reservation> FindByRequestIdAsync(Guid requestId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested || !_byRequestId.TryGetValue(requestId, out var reservation))
            return Task.FromResult(new Reservation());

        return Task.FromResult(reservation);
    }

    public Task<bool> AddAsync(Reservation reservation, CancellationToken cancellationToken)
    {
        var saved = reservation is not null && reservation.Id != Guid.Empty &&
            reservation.RequestId != Guid.Empty && _byRequestId.TryAdd(reservation.RequestId, reservation);
        return Task.FromResult(saved);
    }
}
