namespace Oib.Vezba03.Domain.Configuration;

public sealed record SecurityConfiguration(
    int MinimumPasswordLength,
    bool RequireMfaForAdmins);
