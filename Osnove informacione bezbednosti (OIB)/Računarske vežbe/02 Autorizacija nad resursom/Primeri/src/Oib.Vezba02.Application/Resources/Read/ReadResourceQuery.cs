using Oib.Vezba02.Domain.Access;

namespace Oib.Vezba02.Application.Resources.Read;

public sealed record ReadResourceQuery(
    Requester Requester,
    string ResourceId);
