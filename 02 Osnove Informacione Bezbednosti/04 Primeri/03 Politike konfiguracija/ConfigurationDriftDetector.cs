namespace Oib.Vezba03;

public sealed class ConfigurationDriftDetector
{
    public IReadOnlyList<ConfigurationFinding> Evaluate(SecurityBaseline baseline, SecurityConfiguration current)
    {
        var findings = new List<ConfigurationFinding>();
        if (current.MinimumPasswordLength < baseline.MinimumPasswordLength)
            findings.Add(new("password.minLength", baseline.MinimumPasswordLength.ToString(), current.MinimumPasswordLength.ToString()));
        if (current.RequireMfaForAdmins != baseline.RequireMfaForAdmins)
            findings.Add(new("admin.requireMfa", baseline.RequireMfaForAdmins.ToString(), current.RequireMfaForAdmins.ToString()));
        return findings;
    }
}

