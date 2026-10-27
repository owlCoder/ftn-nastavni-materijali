using Oib.Vezba06.Domain.Signals;

namespace Oib.Vezba06.Domain.Detection;

public sealed class RepeatedFailedLoginRule : IDetectionRule
{
    public const string Name = "repeated-failed-login";

    private readonly int _threshold;
    private readonly TimeSpan _window;

    public RepeatedFailedLoginRule(int threshold, TimeSpan window)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(threshold, 1);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(window, TimeSpan.Zero);

        _threshold = threshold;
        _window = window;
    }

    public IReadOnlyList<SecuritySignal> Detect(
        IReadOnlyCollection<LoginAttempt> attempts,
        DateTimeOffset now)
    {
        ArgumentNullException.ThrowIfNull(attempts);

        return attempts
            .Where(attempt => !attempt.Succeeded && IsInsideWindow(attempt, now))
            .GroupBy(attempt => (attempt.SubjectId, attempt.SourceIp))
            .Where(group => group.Count() >= _threshold)
            .Select(group => new SecuritySignal(
                Name,
                group.Key.SubjectId,
                group.Key.SourceIp,
                group.Count()))
            .ToArray();
    }

    private bool IsInsideWindow(LoginAttempt attempt, DateTimeOffset now) =>
        attempt.Timestamp <= now && now - attempt.Timestamp <= _window;
}
