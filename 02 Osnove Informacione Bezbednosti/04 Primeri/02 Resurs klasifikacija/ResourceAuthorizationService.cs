namespace Oib.Vezba02;

public sealed class ResourceAuthorizationService
{
    public bool CanRead(AccessRequest request, ProtectedResource resource)
    {
        if (!request.Permissions.Contains("records:read")) return false;
        if (resource.OwnerId == request.ActorId) return true;

        return request.Permissions.Contains("records:read:any") &&
               resource.Classification != DataClassification.Restricted;
    }
}

