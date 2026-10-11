using Application.Customer.Queries;
using RepoInMemory.Common.Queries;
using Domain;

namespace RepoInMemory.Customer.Queries
{
    public class CustomerFindByIdQuery : BaseFindByIdQuery<CustomerEntity>, ICustomerFindByIdQuery { }
}
