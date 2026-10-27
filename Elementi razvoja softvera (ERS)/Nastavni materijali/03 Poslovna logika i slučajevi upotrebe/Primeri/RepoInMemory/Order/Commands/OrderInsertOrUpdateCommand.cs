using Application.Common.Commands;
using Application.Common.Queries;
using Application.Order.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Order.Commands
{
    public class OrderInsertOrUpdateCommand : BaseInsertOrUpdateCommand<OrderEntity>, IOrderInsertOrUpdateCommand
    {
        public OrderInsertOrUpdateCommand(IFindByIdQuery<OrderEntity> findByIdQuery, IInsertCommand<OrderEntity> insertCommand, IUpdateCommand<OrderEntity> updateCommand) : base(findByIdQuery, insertCommand, updateCommand) { }
    }
}
