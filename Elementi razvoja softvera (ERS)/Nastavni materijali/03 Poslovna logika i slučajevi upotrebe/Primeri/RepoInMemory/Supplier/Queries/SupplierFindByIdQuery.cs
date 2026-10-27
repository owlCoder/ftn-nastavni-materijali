using Application.Supplier.Queries;
using RepoInMemory.Common.Queries;
using Domain;

namespace RepoInMemory.Supplier.Queries
{
    public class SupplierFindByIdQuery : BaseFindByIdQuery<SupplierEntity>, ISupplierFindByIdQuery { }
}
