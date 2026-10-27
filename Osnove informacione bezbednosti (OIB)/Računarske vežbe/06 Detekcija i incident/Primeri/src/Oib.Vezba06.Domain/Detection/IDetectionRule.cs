using Oib.Vezba06.Domain.Signals;

namespace Oib.Vezba06.Domain.Detection;

public interface IDetectionRule
{
    IReadOnlyList<SecuritySignal> Detect(
        IReadOnlyCollection<LoginAttempt> attempts,
        DateTimeOffset now);
}
