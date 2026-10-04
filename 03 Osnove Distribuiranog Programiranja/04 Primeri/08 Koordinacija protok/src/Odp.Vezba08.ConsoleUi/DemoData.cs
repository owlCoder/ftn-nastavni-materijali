namespace Odp.Vezba08.ConsoleUi;

public static class DemoData
{
    public const string Resource = "raspored:GS-NOVI-SAD";

    public const int QueueCapacity = 2;

    public static readonly DateTimeOffset Start = new(2027, 3, 22, 9, 0, 0, TimeSpan.Zero);

    public static readonly TimeSpan LeaseDuration = TimeSpan.FromSeconds(30);
}
