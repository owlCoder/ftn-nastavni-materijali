using System.Collections.Concurrent;
using EquipmentReservation.Application.Ports.Reservations;
using EquipmentReservation.Domain.Reservations;

namespace EquipmentReservation.Infrastructure.Persistence;

public sealed class InMemoryReservationRepository : IReservationRepository
{
    private readonly ConcurrentDictionary<Guid, Reservation> _byRequestId = new();

    public Task<Reservation?> FindByRequestIdAsync(
        Guid requestId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _byRequestId.TryGetValue(requestId, out var reservation);
        return Task.FromResult(reservation);
    }

    public Task AddAsync(
        Reservation reservation,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(reservation);
        cancellationToken.ThrowIfCancellationRequested();

        if (!_byRequestId.TryAdd(reservation.RequestId, reservation))
            throw new InvalidOperationException(
                "A reservation with the same request id already exists.");

        return Task.CompletedTask;
    }
}
