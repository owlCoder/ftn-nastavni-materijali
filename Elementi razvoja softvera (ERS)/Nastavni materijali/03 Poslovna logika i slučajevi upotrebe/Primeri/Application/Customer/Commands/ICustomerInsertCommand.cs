using Application.Common.Commands;
using Domain;

namespace Application.Customer.Commands
{
    public interface ICustomerInsertCommand : IInsertCommand<CustomerEntity>
    {
    }
}
