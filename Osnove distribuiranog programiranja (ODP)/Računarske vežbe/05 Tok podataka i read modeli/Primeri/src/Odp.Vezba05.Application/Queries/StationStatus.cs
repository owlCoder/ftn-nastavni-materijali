using Odp.Vezba05.Domain.ReadModels;

namespace Odp.Vezba05.Application.Queries;

public sealed record StationStatus(StationView View, bool IsStale, TimeSpan Age);
