using Domain;
using Application.Common.Interfaces;

namespace IBusinessImpl.Order
{
    public interface IOrderCreator
    {
        OrderEntity ApplyShoppingCart();
        ITimeAssigner TimeAssigner { get; set; }
    }
}
