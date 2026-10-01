using Oib.Vezba06.Domain.Signals;

namespace Oib.Vezba06.ConsoleUi;

public static class DemoData
{
    public const int DetectionThreshold = 3;
    public const int HighSeverityFrom = 5;

    public static TimeSpan DetectionWindow { get; } = TimeSpan.FromMinutes(10);

    public static IReadOnlyList<LoginAttempt> AttemptsAt(DateTimeOffset now) =>
    [
        .. Failures("ana", "203.0.113.10", count: 5, now.AddMinutes(-2)),
        .. Failures("marko", "198.51.100.7", count: 3, now.AddMinutes(-4)),
        .. Failures("jelena", "192.0.2.44", count: 2, now.AddMinutes(-1)),
        .. Failures("petar", "192.0.2.90", count: 6, now.AddHours(-3)),
        new LoginAttempt("jelena", "192.0.2.44", Succeeded: true, now)
    ];

    private static IEnumerable<LoginAttempt> Failures(
        string subjectId,
        string sourceIp,
        int count,
        DateTimeOffset timestamp) =>
        Enumerable.Range(0, count)
            .Select(_ => new LoginAttempt(subjectId, sourceIp, Succeeded: false, timestamp));
}
