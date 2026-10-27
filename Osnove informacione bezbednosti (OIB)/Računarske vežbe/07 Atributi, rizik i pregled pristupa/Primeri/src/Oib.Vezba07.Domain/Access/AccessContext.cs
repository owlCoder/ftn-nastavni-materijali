namespace Oib.Vezba07.Domain.Access;

public sealed record AccessContext(
    string SubjectId,
    string Role,
    string Permission,
    bool ManagedDevice,
    string Location,
    bool RestrictedResource,
    int RiskScore);
