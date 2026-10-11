using Application.Common.Commands;
using Application.Common.Queries;
using Application.Customer.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Customer.Commands
{
    public class CustomerInsertIfNotExistsCommand : BaseInsertIfNotExistsCommand<CustomerEntity>, ICustomerInsertIfNotExistsCommand
    {
        public CustomerInsertIfNotExistsCommand(IFindByIdQuery<CustomerEntity> findByIdQuery, IInsertCommand<CustomerEntity> insertCommand) : base(findByIdQuery, insertCommand) { }
    }
}
