using Odp.Vezba01.Domain.Nodes;

namespace Odp.Vezba01.Application.Ports;

public interface INodeRegistry
{
    StationNode? Find(string nodeId);

    IReadOnlyList<StationNode> All();

    void Save(StationNode node);
}
