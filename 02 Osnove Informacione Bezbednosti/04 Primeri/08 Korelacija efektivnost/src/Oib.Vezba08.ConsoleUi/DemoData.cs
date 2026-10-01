using Oib.Vezba08.Domain.Events;

namespace Oib.Vezba08.ConsoleUi;

public static class DemoData
{
    public const string Control = "step-up-export";
    public const decimal TargetRatio = 0.5m;

    public static IReadOnlyList<SecurityEvent> Events { get; } =
    [
        new SecurityEvent("corr-42", "login-failed", "ana", Blocked: false),
        new SecurityEvent("corr-42", "step-up-required", "ana", Blocked: true),
        new SecurityEvent("corr-42", "export-denied", "ana", Blocked: true),
        new SecurityEvent("corr-77", "login-failed", "marko", Blocked: false),
        new SecurityEvent("corr-77", "export-denied", "marko", Blocked: true)
    ];
}
