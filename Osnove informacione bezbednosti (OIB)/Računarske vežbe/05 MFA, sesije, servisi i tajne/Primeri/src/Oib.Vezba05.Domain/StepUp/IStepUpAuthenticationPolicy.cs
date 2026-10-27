using Oib.Vezba05.Domain.Operations;
using Oib.Vezba05.Domain.Sessions;

namespace Oib.Vezba05.Domain.StepUp;

public interface IStepUpAuthenticationPolicy
{
    StepUpDecision Evaluate(AuthSession? session, RiskyOperation? operation, DateTimeOffset now);
}
