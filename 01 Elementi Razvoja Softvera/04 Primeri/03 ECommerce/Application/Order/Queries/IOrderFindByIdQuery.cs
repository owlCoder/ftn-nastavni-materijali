using Application.Common.Queries;
using Domain;

namespace Application.Order.Queries
{
    public interface IOrderFindByIdQuery : IFindByIdQuery<OrderEntity>
    {
    }
}
