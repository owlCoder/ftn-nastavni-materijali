using Oib.Vezba06.Domain.Signals;

namespace Oib.Vezba06.Domain.Incidents;

public sealed class IncidentSeverityPolicy(int highSeverityFrom)
{
    public IncidentSeverity For(SecuritySignal signal)
    {
        ArgumentNullException.ThrowIfNull(signal);

        return signal.Count >= highSeverityFrom
            ? IncidentSeverity.High
            : IncidentSeverity.Medium;
    }
}
