namespace Odp.Vezba07.Application.Flushing;

public interface IFlushOutboxUseCase
{
    FlushReport Flush();
}
