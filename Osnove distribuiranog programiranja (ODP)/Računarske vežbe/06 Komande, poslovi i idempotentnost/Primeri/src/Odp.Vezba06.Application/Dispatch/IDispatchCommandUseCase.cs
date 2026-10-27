namespace Odp.Vezba06.Application.Dispatch;

public interface IDispatchCommandUseCase
{
    DispatchResult Dispatch(string commandId, string deviceId, string action);
}
