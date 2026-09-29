namespace Oib.Vezba07;

public sealed class RiskBasedAccessPolicy
{
    public AccessDecision Evaluate(AccessContext context, DateTimeOffset now)
    {
        if (context.Role != "Operator") return new(false, "Uloga nema poslovnu odgovornost.", now.AddDays(1));
        if (context.RestrictedResource && !context.ManagedDevice)
            return new(false, "Ograničen resurs zahteva upravljani uređaj.", now.AddHours(4));
        if (context.RiskScore >= 70) return new(false, "Rizik zahteva dodatnu proveru.", now.AddHours(1));
        return new(true, "Atributi i rizik su prihvatljivi.", now.AddDays(30));
    }
}

