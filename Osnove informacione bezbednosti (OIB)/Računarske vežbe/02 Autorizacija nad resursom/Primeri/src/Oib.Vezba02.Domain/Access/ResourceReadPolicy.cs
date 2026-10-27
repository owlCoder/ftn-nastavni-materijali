using Oib.Vezba02.Domain.Resources;

namespace Oib.Vezba02.Domain.Access;

public sealed class ResourceReadPolicy : IResourceReadPolicy
{
    public ResourceAccessDecision Decide(Requester? requester, ProtectedResource? resource)
    {
        if (requester is null || resource is null || string.IsNullOrWhiteSpace(requester.ActorId) ||
            requester.Permissions is null || string.IsNullOrWhiteSpace(resource.OwnerId) ||
            !Enum.IsDefined(resource.Classification))
            return new(false, ResourceAccessCodes.InvalidRequest);

        if (!requester.Permissions.Contains(Permissions.ReadRecords))
            return new(false, ResourceAccessCodes.MissingReadPermission);
        if (resource.OwnerId == requester.ActorId)
            return new(true, ResourceAccessCodes.OwnerAccess);
        if (!requester.Permissions.Contains(Permissions.ReadAnyRecord))
            return new(false, ResourceAccessCodes.NotOwner);

        return resource.Classification == DataClassification.Restricted
            ? new(false, ResourceAccessCodes.RestrictedToOwner)
            : new(true, ResourceAccessCodes.ElevatedAccess);
    }
}
