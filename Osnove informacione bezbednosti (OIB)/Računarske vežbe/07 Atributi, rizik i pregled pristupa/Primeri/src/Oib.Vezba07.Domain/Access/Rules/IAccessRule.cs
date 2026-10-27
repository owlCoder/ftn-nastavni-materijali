namespace Oib.Vezba07.Domain.Access.Rules;

public interface IAccessRule
{
    AccessDenial? Evaluate(AccessContext context);
}
