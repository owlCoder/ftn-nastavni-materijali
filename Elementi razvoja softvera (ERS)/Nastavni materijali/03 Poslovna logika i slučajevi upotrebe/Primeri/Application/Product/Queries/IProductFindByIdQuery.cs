using Application.Common.Queries;
using Domain;

namespace Application.Product.Queries
{
    public interface IProductFindByIdQuery : IFindByIdQuery<ProductEntity>
    {
    }
}
