using Odp.Vezba01.Application.Ports;
using Odp.Vezba01.Domain.Nodes;

namespace Odp.Vezba01.Infrastructure.Nodes;

public sealed class InMemoryNodeRegistry : INodeRegistry
{
    private readonly Dictionary<string, StationNode> _nodes = new(StringComparer.Ordinal);

    public InMemoryNodeRegistry(IEnumerable<StationNode> nodes)
    {
        ArgumentNullException.ThrowIfNull(nodes);
        foreach (var node in nodes)
            _nodes[node.NodeId] = node;
    }

    public StationNode? Find(string nodeId) => _nodes.GetValueOrDefault(nodeId);

    public IReadOnlyList<StationNode> All() => _nodes.Values.OrderBy(node => node.NodeId).ToArray();

    public void Save(StationNode node) => _nodes[node.NodeId] = node;
}
