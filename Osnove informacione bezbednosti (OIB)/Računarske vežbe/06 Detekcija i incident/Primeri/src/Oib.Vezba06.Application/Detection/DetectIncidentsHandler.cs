using Oib.Vezba06.Application.Ports;
using Oib.Vezba06.Domain.Detection;
using Oib.Vezba06.Domain.Incidents;

namespace Oib.Vezba06.Application.Detection;

public sealed class DetectIncidentsHandler(
    ILoginAttemptSource attemptSource,
    IEnumerable<IDetectionRule> rules,
    IncidentFactory incidentFactory,
    IIncidentIdGenerator incidentIds,
    IIncidentRepository incidents,
    IClock clock) : IDetectIncidentsUseCase
{
    private readonly IReadOnlyList<IDetectionRule> _rules = rules.ToArray();

    public IReadOnlyList<Incident> Detect()
    {
        var now = clock.UtcNow;
        var attempts = attemptSource.GetAttempts();

        var opened = _rules
            .SelectMany(rule => rule.Detect(attempts, now))
            .Select(signal => incidentFactory.Open(incidentIds.NextId(), signal, now))
            .ToArray();

        foreach (var incident in opened)
            incidents.Add(incident);

        return opened;
    }
}
