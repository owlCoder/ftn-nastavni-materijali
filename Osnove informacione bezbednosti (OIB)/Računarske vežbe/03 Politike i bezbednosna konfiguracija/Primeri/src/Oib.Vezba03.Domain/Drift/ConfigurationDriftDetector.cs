using Oib.Vezba03.Domain.Configuration;

namespace Oib.Vezba03.Domain.Drift;

public sealed class ConfigurationDriftDetector(IEnumerable<IConfigurationControl> controls)
{
    private readonly IReadOnlyList<IConfigurationControl> _controls = controls.ToArray();

    public IReadOnlyList<ConfigurationFinding> Evaluate(
        SecurityBaseline baseline,
        SecurityConfiguration current)
    {
        ArgumentNullException.ThrowIfNull(baseline);
        ArgumentNullException.ThrowIfNull(current);

        return _controls
            .Select(control => control.Check(baseline, current))
            .OfType<ConfigurationFinding>()
            .ToArray();
    }
}
