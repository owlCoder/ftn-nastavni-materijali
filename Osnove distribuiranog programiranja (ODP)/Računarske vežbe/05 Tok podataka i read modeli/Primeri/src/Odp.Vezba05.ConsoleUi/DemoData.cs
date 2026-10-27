using Odp.Vezba05.Domain.Telemetry;

namespace Odp.Vezba05.ConsoleUi;

public static class DemoData
{
    public const string Station = "GS-NOVI-SAD";

    public static readonly DateTimeOffset Start = new(2027, 3, 1, 9, 0, 0, TimeSpan.Zero);

    public static readonly TimeSpan StaleAfter = TimeSpan.FromSeconds(30);

    public static TelemetryReading Reading(long sequence, double signal) =>
        new(Station, sequence, Start.AddSeconds(5 * sequence), signal);
}
