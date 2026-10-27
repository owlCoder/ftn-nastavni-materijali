using Domain;

namespace Application.Order.Interfaces
{
    public interface IDiscountCreator
    {
        IDiscount GetDiscount(OrderEntity order);
    }
}
