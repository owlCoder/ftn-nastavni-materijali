namespace Oib.Vezba03.Domain.Configuration;

public sealed record SecurityBaseline(
    string Version,
    int MinimumPasswordLength,
    bool RequireMfaForAdmins);
