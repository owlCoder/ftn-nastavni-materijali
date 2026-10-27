using Oib.Vezba01.Domain.Authorization;

namespace Oib.Vezba01.Application.Access;

public interface IAuthorizeAccessUseCase
{
    AccessDecision Authorize(AccessRequest request);
}
