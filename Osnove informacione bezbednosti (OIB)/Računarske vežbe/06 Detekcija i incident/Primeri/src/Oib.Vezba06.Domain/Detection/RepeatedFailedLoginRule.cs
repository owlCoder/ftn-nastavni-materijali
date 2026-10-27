using Oib.Vezba06.Domain.Signals;
using Oib.Vezba06.Domain.Shared;

namespace Oib.Vezba06.Domain.Detection;

public sealed class RepeatedFailedLoginRule : IDetectionRule
{
    public const string Name = "repeated-failed-login";

    private readonly int _threshold;
    private readonly TimeSpan _window;

    public RepeatedFailedLoginRule(int threshold, TimeSpan window)
    {
        _threshold = threshold;
        _window = window;
    }

    public Result<IReadOnlyList<SecuritySignal>> Detect(
        IReadOnlyCollection<LoginAttempt>? attempts,
        DateTimeOffset now)
    {
        if (_threshold < 1 || _window <= TimeSpan.Zero)
            return Result<IReadOnlyList<SecuritySignal>>.Fail("InvalidDetectionConfiguration");
        if (attempts is null || attempts.Any(a => a is null || string.IsNullOrWhiteSpace(a.SubjectId) || string.IsNullOrWhiteSpace(a.SourceIp)))
            return Result<IReadOnlyList<SecuritySignal>>.Fail("InvalidAttempts");
        IReadOnlyList<SecuritySignal> signals = attempts
            .Where(attempt => !attempt.Succeeded && IsInsideWindow(attempt, now))
            .GroupBy(attempt => (attempt.SubjectId, attempt.SourceIp))
            .Where(group => group.Count() >= _threshold)
            .Select(group => new SecuritySignal(
                Name,
                group.Key.SubjectId,
                group.Key.SourceIp,
                group.Count()))
            .ToArray();
        return Result<IReadOnlyList<SecuritySignal>>.Ok(signals);
    }

    private bool IsInsideWindow(LoginAttempt attempt, DateTimeOffset now) =>
        attempt.Timestamp <= now && now - attempt.Timestamp <= _window;
}
