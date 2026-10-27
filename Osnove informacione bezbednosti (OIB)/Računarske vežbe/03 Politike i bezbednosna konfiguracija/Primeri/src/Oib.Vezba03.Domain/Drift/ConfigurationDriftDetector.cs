using Oib.Vezba03.Domain.Configuration;
using Oib.Vezba03.Domain.Shared;
namespace Oib.Vezba03.Domain.Drift;
public sealed class ConfigurationDriftDetector(IEnumerable<IConfigurationControl>? controls) : IConfigurationDriftDetector
{
    private readonly IReadOnlyList<IConfigurationControl> _controls = (controls ?? []).ToArray();
    public Result<IReadOnlyList<ConfigurationFinding>> Evaluate(SecurityBaseline? baseline, SecurityConfiguration? current)
    {
        if (baseline is null || current is null || string.IsNullOrWhiteSpace(baseline.Version) ||
            baseline.MinimumPasswordLength < 1 || current.MinimumPasswordLength < 1)
            return Result<IReadOnlyList<ConfigurationFinding>>.Fail("InvalidConfiguration");
        if (_controls.Count == 0 || _controls.Any(c => c is null))
            return Result<IReadOnlyList<ConfigurationFinding>>.Fail("MissingControls");
        IReadOnlyList<ConfigurationFinding> findings = _controls.Select(c => c.Check(baseline, current))
            .OfType<ConfigurationFinding>().ToArray();
        return Result<IReadOnlyList<ConfigurationFinding>>.Ok(findings);
    }
}
