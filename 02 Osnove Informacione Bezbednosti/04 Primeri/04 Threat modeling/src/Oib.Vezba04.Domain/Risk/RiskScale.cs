namespace Oib.Vezba04.Domain.Risk;

public static class RiskScale
{
    public const int Minimum = 1;
    public const int Maximum = 5;

    public static bool Contains(int value) => value is >= Minimum and <= Maximum;
}
