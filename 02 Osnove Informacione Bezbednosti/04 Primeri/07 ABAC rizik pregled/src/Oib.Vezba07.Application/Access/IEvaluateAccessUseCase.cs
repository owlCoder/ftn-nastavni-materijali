using Oib.Vezba07.Domain.Access;

namespace Oib.Vezba07.Application.Access;

public interface IEvaluateAccessUseCase
{
    AccessDecision Evaluate(EvaluateAccessRequest request);
}
