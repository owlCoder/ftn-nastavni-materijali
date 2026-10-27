using Oib.Vezba06.Domain.Signals;
using Oib.Vezba06.Domain.Shared;
namespace Oib.Vezba06.Domain.Detection;
public interface IDetectionRule
{
    Result<IReadOnlyList<SecuritySignal>> Detect(IReadOnlyCollection<LoginAttempt>? attempts, DateTimeOffset now);
}
