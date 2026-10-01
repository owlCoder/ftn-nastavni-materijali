namespace Oib.Vezba06.Domain.Signals;

public sealed record SecuritySignal(
    string Rule,
    string SubjectId,
    string SourceIp,
    int Count);
