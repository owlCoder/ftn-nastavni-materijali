using Oib.Vezba06.Domain.Signals;

namespace Oib.Vezba06.Domain.Incidents;

public sealed class IncidentFactory(IncidentSeverityPolicy severityPolicy)
{
    public Incident Open(string id, SecuritySignal signal, DateTimeOffset openedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(signal);

        return new Incident(
            id,
            severityPolicy.For(signal),
            $"{signal.Rule}: {signal.SubjectId} sa {signal.SourceIp} ({signal.Count} pokušaja)",
            IncidentStatus.Open,
            openedAt);
    }
}
