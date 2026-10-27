using Oib.Vezba03.Domain.Configuration;

namespace Oib.Vezba03.Domain.Drift.Controls;

public sealed class AdminMfaControl : IConfigurationControl
{
    public const string Name = "admin.requireMfa";

    public ConfigurationFinding? Check(SecurityBaseline baseline, SecurityConfiguration current) =>
        baseline.RequireMfaForAdmins && !current.RequireMfaForAdmins
            ? new(
                Name,
                baseline.RequireMfaForAdmins.ToString(),
                current.RequireMfaForAdmins.ToString())
            : null;
}
