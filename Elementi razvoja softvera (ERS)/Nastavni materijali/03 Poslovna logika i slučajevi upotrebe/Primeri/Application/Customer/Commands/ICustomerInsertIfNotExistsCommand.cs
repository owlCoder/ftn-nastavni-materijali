using Application.Common.Commands;
using Domain;

namespace Application.Customer.Commands
{
    public interface ICustomerInsertIfNotExistsCommand: IInsertIfNotExistsCommand<CustomerEntity>
    {
    }
}
