using Application.Common.Commands;
using Application.Common.Queries;
using Application.Product.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Product.Commands
{
    public class ProductInsertOrUpdateCommand : BaseInsertOrUpdateCommand<ProductEntity>, IProductInsertOrUpdateCommand
    {
        public ProductInsertOrUpdateCommand(IFindByIdQuery<ProductEntity> findByIdQuery, IInsertCommand<ProductEntity> insertCommand, IUpdateCommand<ProductEntity> updateCommand) : base(findByIdQuery, insertCommand, updateCommand) { }
    }
}
