namespace Oib.Vezba06;

public sealed class IncidentFactory
{
    public Incident Open(SecuritySignal signal) => new(
        $"INC-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}",
        signal.Count >= 5 ? "high" : "medium",
        $"{signal.Rule}: {signal.SubjectId} sa {signal.SourceIp}",
        "open");
}

