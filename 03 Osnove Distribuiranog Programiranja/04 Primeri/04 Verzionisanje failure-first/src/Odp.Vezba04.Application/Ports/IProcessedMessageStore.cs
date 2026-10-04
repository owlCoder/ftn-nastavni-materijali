namespace Odp.Vezba04.Application.Ports;

public interface IProcessedMessageStore
{
    bool Contains(string messageId);

    void Add(string messageId);
}
