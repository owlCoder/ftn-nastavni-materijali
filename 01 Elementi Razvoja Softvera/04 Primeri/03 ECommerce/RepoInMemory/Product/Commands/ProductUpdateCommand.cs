using Application.Product.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Product.Commands
{
    public class ProductUpdateCommand : BaseUpdateCommand<ProductEntity>, IProductUpdateCommand { }
}
