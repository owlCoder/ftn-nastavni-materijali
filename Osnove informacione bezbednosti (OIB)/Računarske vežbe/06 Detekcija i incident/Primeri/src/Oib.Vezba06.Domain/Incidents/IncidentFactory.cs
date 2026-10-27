using Oib.Vezba06.Domain.Signals;
using Oib.Vezba06.Domain.Shared;
namespace Oib.Vezba06.Domain.Incidents;
public sealed class IncidentFactory(IIncidentSeverityPolicy severityPolicy) : IIncidentFactory
{
    public Result<Incident> Open(string? id, SecuritySignal? signal, DateTimeOffset openedAt)
    {
        if (string.IsNullOrWhiteSpace(id) || signal is null || string.IsNullOrWhiteSpace(signal.Rule) ||
            string.IsNullOrWhiteSpace(signal.SubjectId) || string.IsNullOrWhiteSpace(signal.SourceIp))
            return Result<Incident>.Fail("InvalidIncidentInput");
        var severity = severityPolicy.For(signal);
        if (!severity.Success) return Result<Incident>.Fail(severity.Error ?? "SeverityFailed");
        return Result<Incident>.Ok(new Incident(id, severity.Value,
            $"{signal.Rule}: {signal.SubjectId} sa {signal.SourceIp} ({signal.Count} pokušaja)",
            IncidentStatus.Open, openedAt));
    }
}
