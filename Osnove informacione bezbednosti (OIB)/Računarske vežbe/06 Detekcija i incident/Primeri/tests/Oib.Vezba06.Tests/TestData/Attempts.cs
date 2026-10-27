using Oib.Vezba06.Domain.Signals;

namespace Oib.Vezba06.Tests.TestData;

internal static class Attempts
{
    public static LoginAttempt[] Failures(
        string subjectId,
        string sourceIp,
        int count,
        DateTimeOffset timestamp) =>
        Enumerable.Range(0, count)
            .Select(_ => new LoginAttempt(subjectId, sourceIp, Succeeded: false, timestamp))
            .ToArray();
}
