using Oib.Vezba07.Domain.Access;

namespace Oib.Vezba07.Application.Access;

public sealed record EvaluateAccessRequest(
    AccessContext Context,
    string BusinessOwner);
