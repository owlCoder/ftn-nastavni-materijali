using Application.Product.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Product.Commands
{
    public class ProductDeleteCommand : BaseDeleteCommand<ProductEntity>, IProductDeleteCommand
    {
    }
}
