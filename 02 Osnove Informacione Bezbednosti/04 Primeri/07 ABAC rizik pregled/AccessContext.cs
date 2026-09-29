namespace Oib.Vezba07;

public sealed record AccessContext(string Role, bool ManagedDevice, string Location, bool RestrictedResource, int RiskScore);

