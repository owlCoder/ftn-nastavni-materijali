using Application.Common.Commands;
using Domain;

namespace Application.Order.Commands
{
    public interface IOrderDeleteCommand: IDeleteCommand<OrderEntity>
    {
    }
}
