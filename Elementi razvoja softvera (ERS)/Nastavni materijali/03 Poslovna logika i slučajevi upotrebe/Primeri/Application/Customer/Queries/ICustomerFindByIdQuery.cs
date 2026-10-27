using Application.Common.Queries;
using Domain;

namespace Application.Customer.Queries
{
    public interface ICustomerFindByIdQuery : IFindByIdQuery<CustomerEntity>
    {
    }
}
