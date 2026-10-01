namespace Oib.Vezba01.Domain.Authorization;

public static class AccessDecisionCodes
{
    public const string NotAuthenticated = "NotAuthenticated";
    public const string PermissionNotGranted = "PermissionNotGranted";
    public const string GrantedByRole = "GrantedByRole";
}
