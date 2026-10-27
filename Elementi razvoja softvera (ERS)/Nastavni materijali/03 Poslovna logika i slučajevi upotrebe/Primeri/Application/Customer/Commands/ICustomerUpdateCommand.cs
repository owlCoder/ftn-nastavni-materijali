using Application.Common.Commands;
using Domain;

namespace Application.Customer.Commands
{
    public interface ICustomerUpdateCommand: IUpdateCommand<CustomerEntity>
    {
    }
}
