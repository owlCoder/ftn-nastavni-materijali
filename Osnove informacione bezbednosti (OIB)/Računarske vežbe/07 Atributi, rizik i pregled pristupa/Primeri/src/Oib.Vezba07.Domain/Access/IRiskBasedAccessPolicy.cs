namespace Oib.Vezba07.Domain.Access;

public interface IRiskBasedAccessPolicy
{
    AccessDecision Evaluate(AccessContext? context, DateTimeOffset now);
}
