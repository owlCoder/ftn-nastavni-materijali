using Oib.Vezba02.Domain.Resources;

namespace Oib.Vezba02.Domain.Access;

public interface IResourceReadPolicy
{
    ResourceAccessDecision Decide(Requester? requester, ProtectedResource? resource);
}
