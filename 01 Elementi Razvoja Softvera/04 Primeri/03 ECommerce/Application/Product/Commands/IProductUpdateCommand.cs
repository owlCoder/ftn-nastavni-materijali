using Application.Common.Commands;
using Domain;

namespace Application.Product.Commands
{
    public interface IProductUpdateCommand: IUpdateCommand<ProductEntity>
    {
    }
}
