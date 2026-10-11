using Application.Product.Queries;
using RepoInMemory.Common.Queries;
using Domain;

namespace RepoInMemory.Product.Queries
{
    public class ProductFindByIdQuery : BaseFindByIdQuery<ProductEntity>, IProductFindByIdQuery { }
}
