using Application.Common.Commands;
using Domain;

namespace Application.Customer.Commands
{
    public interface ICustomerDeleteCommand: IDeleteCommand<CustomerEntity>
    {
    }
}
