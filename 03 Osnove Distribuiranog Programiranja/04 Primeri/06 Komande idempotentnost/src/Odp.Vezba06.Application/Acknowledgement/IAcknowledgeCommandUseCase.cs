namespace Odp.Vezba06.Application.Acknowledgement;

public interface IAcknowledgeCommandUseCase
{
    string Acknowledge(string commandId);
}
