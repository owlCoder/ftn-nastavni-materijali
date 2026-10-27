using Oib.Vezba02.Domain.Resources;

namespace Oib.Vezba02.Application.Resources.Read;

public sealed record ReadResourceResult(
    ReadResourceOutcome Outcome,
    string Code,
    ProtectedResource? Resource);
