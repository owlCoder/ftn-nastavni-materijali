using Application.Customer.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Customer.Commands
{
    public class CustomerInsertCommand : BaseInsertCommand<CustomerEntity>, ICustomerInsertCommand
    {
    }
}
