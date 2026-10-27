using Application.Common.Commands;
using Application.Common.Queries;
using Application.Product.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Product.Commands
{
    public class ProductInsertIfNotExistsCommand : BaseInsertIfNotExistsCommand<ProductEntity>, IProductInsertIfNotExistsCommand
    {
        public ProductInsertIfNotExistsCommand(IFindByIdQuery<ProductEntity> findByIdQuery, IInsertCommand<ProductEntity> insertCommand) : base(findByIdQuery, insertCommand) { }
    }
}
