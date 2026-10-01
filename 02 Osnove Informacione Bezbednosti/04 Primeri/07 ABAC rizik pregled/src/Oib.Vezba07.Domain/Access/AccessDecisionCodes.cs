namespace Oib.Vezba07.Domain.Access;

public static class AccessDecisionCodes
{
    public const string Granted = "Granted";
    public const string RoleNotResponsible = "RoleNotResponsible";
    public const string UnmanagedDevice = "UnmanagedDevice";
    public const string UntrustedLocation = "UntrustedLocation";
    public const string RiskTooHigh = "RiskTooHigh";
}
