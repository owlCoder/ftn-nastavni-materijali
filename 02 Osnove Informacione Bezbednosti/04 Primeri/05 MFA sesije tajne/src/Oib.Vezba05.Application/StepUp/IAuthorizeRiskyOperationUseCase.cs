using Oib.Vezba05.Domain.StepUp;

namespace Oib.Vezba05.Application.StepUp;

public interface IAuthorizeRiskyOperationUseCase
{
    StepUpDecision Authorize(AuthorizeOperationRequest request);
}
