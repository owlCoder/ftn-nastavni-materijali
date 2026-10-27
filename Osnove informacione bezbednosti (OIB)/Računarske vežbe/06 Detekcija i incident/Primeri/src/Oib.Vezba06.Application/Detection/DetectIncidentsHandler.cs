using Oib.Vezba06.Application.Ports;
using Oib.Vezba06.Domain.Detection;
using Oib.Vezba06.Domain.Incidents;
using Oib.Vezba06.Domain.Shared;
namespace Oib.Vezba06.Application.Detection;
public sealed class DetectIncidentsHandler(ILoginAttemptSource attemptSource, IEnumerable<IDetectionRule>? rules,
    IIncidentFactory incidentFactory, IIncidentIdGenerator incidentIds, IIncidentRepository incidents, IClock clock) : IDetectIncidentsUseCase
{
    private readonly IReadOnlyList<IDetectionRule> _rules = (rules ?? []).ToArray();
    public Result<IReadOnlyList<Incident>> Detect()
    {
        if (_rules.Count == 0 || _rules.Any(rule => rule is null)) return Result<IReadOnlyList<Incident>>.Fail("MissingDetectionRules");
        var now = clock.UtcNow;
        var attempts = attemptSource.GetAttempts();
        var opened = new List<Incident>();
        foreach (var rule in _rules)
        {
            var detection = rule.Detect(attempts, now);
            if (!detection.Success || detection.Value is null)
                return Result<IReadOnlyList<Incident>>.Fail(detection.Error ?? "DetectionFailed");
            foreach (var signal in detection.Value)
            {
                var incident = incidentFactory.Open(incidentIds.NextId(), signal, now);
                if (!incident.Success || incident.Value is null)
                    return Result<IReadOnlyList<Incident>>.Fail(incident.Error ?? "IncidentCreationFailed");
                opened.Add(incident.Value);
            }
        }
        foreach (var incident in opened)
        {
            var saved = incidents.Add(incident);
            if (!saved.Success) return Result<IReadOnlyList<Incident>>.Fail(saved.Error ?? "IncidentStorageFailed");
        }
        return Result<IReadOnlyList<Incident>>.Ok(opened);
    }
}
