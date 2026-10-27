namespace Odp.Vezba06.Application.Retries;

public interface IRetryTimedOutCommandsUseCase
{
    IReadOnlyList<RetryOutcome> Run();
}
