namespace Oib.Vezba03;

public sealed record SecurityBaseline(string Version, int MinimumPasswordLength, bool RequireMfaForAdmins);

