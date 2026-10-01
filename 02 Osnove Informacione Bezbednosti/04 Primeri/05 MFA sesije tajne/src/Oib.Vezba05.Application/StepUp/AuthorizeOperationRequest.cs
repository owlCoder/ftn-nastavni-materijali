using Oib.Vezba05.Domain.Operations;

namespace Oib.Vezba05.Application.StepUp;

public sealed record AuthorizeOperationRequest(
    string SessionId,
    RiskyOperation Operation);
