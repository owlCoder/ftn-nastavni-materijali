using Application.Common.Commands;
using Application.Common.Queries;
using Application.Order.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Order.Commands
{
    public class OrderInsertIfNotExistsCommand : BaseInsertIfNotExistsCommand<OrderEntity>, IOrderInsertIfNotExistsCommand
    {
        public OrderInsertIfNotExistsCommand(IFindByIdQuery<OrderEntity> findByIdQuery, IInsertCommand<OrderEntity> insertCommand) : base(findByIdQuery, insertCommand) { }
    }
}
