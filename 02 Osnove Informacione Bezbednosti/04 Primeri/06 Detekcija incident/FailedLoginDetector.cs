namespace Oib.Vezba06;

public sealed class FailedLoginDetector
{
    public SecuritySignal? Detect(IEnumerable<LoginAttempt> attempts, int threshold)
    {
        var group = attempts.Where(item => !item.Succeeded)
            .GroupBy(item => new { item.SubjectId, item.SourceIp })
            .OrderByDescending(items => items.Count())
            .FirstOrDefault();

        return group is not null && group.Count() >= threshold
            ? new("repeated-failed-login", group.Key.SubjectId, group.Key.SourceIp, group.Count())
            : null;
    }
}

