using Oib.Vezba05.Domain.Operations;
using Oib.Vezba05.Domain.Sessions;

namespace Oib.Vezba05.ConsoleUi;

public static class DemoData
{
    public const string FreshSessionId = "sesija-sveza";
    public const string StaleSessionId = "sesija-stara";
    public const string PasswordOnlySessionId = "sesija-bez-mfa";
    public const string RevokedSessionId = "sesija-opozvana";

    public static RiskyOperation ConfidentialExport { get; } =
        new("Izvoz poverljivih podataka", TimeSpan.FromMinutes(10));

    public static IReadOnlyList<AuthSession> SessionsAt(DateTimeOffset now) =>
    [
        new AuthSession(FreshSessionId, "ana", now.AddHours(-2), now.AddMinutes(-2), false),
        new AuthSession(StaleSessionId, "ana", now.AddHours(-2), now.AddMinutes(-45), false),
        new AuthSession(PasswordOnlySessionId, "marko", now.AddMinutes(-5), null, false),
        new AuthSession(RevokedSessionId, "jelena", now.AddHours(-1), now.AddMinutes(-1), true)
    ];
}
