using Odp.Vezba06.Domain.Commands;

namespace Odp.Vezba06.ConsoleUi;

public static class DemoData
{
    public static readonly DateTimeOffset Start = new(2027, 3, 8, 9, 0, 0, TimeSpan.Zero);

    public static readonly RetryRules Rules = new(TimeSpan.FromSeconds(10), MaxAttempts: 2);
}
