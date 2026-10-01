using Oib.Vezba03.Domain.Drift;

namespace Oib.Vezba03.Application.Drift;

public sealed record DriftReport(
    string CorrelationId,
    string BaselineVersion,
    IReadOnlyList<ConfigurationFinding> Findings)
{
    public bool IsCompliant => Findings.Count == 0;
}
