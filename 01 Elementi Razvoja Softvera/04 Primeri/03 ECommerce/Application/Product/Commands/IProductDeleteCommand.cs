using Application.Common.Commands;
using Domain;

namespace Application.Product.Commands
{
    public interface IProductDeleteCommand: IDeleteCommand<ProductEntity>
    {
    }
}
