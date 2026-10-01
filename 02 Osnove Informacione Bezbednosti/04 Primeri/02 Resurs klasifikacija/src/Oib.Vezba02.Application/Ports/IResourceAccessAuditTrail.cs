using Oib.Vezba02.Application.Resources.Read;

namespace Oib.Vezba02.Application.Ports;

public interface IResourceAccessAuditTrail
{
    IReadOnlyList<ResourceAccessAuditEntry> Entries { get; }
}
