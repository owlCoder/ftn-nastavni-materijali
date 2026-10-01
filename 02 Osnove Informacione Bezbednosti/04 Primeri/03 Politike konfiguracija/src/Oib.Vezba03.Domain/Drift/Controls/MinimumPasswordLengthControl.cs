using Oib.Vezba03.Domain.Configuration;

namespace Oib.Vezba03.Domain.Drift.Controls;

public sealed class MinimumPasswordLengthControl : IConfigurationControl
{
    public const string Name = "password.minLength";

    public ConfigurationFinding? Check(SecurityBaseline baseline, SecurityConfiguration current) =>
        current.MinimumPasswordLength < baseline.MinimumPasswordLength
            ? new(
                Name,
                baseline.MinimumPasswordLength.ToString(),
                current.MinimumPasswordLength.ToString())
            : null;
}
