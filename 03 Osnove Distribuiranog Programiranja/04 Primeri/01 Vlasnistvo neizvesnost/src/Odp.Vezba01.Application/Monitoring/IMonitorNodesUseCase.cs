namespace Odp.Vezba01.Application.Monitoring;

public interface IMonitorNodesUseCase
{
    IReadOnlyList<NodeStatusView> Assess();
}
