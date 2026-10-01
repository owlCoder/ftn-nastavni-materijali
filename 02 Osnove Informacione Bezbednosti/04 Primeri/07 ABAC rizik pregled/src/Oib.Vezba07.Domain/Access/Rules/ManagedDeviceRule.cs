namespace Oib.Vezba07.Domain.Access.Rules;

public sealed class ManagedDeviceRule : IAccessRule
{
    public AccessDenial? Evaluate(AccessContext context) =>
        context.RestrictedResource && !context.ManagedDevice
            ? new(
                AccessDecisionCodes.UnmanagedDevice,
                "Ograničen resurs zahteva upravljani uređaj.",
                TimeSpan.FromHours(4))
            : null;
}
