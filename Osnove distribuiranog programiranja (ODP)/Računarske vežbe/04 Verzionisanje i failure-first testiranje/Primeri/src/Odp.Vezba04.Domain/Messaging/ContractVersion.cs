namespace Odp.Vezba04.Domain.Messaging;

public readonly record struct ContractVersion(int Major, int Minor)
{
    public override string ToString() => $"{Major}.{Minor}";
}
