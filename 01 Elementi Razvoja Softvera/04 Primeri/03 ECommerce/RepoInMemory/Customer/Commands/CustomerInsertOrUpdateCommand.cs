using Application.Common.Commands;
using Application.Common.Queries;
using Application.Customer.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Customer.Commands
{
    public class CustomerInsertOrUpdateCommand : BaseInsertOrUpdateCommand<CustomerEntity>, ICustomerInsertOrUpdateCommand
    {
        public CustomerInsertOrUpdateCommand(IFindByIdQuery<CustomerEntity> findByIdQuery, IInsertCommand<CustomerEntity> insertCommand, IUpdateCommand<CustomerEntity> updateCommand) : base(findByIdQuery, insertCommand, updateCommand) { }
    }
}
