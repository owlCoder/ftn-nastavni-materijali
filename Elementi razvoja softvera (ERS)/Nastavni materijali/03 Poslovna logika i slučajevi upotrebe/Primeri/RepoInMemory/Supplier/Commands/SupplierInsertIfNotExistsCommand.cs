using Application.Common.Commands;
using Application.Common.Queries;
using Application.Supplier.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Supplier.Commands
{
    public class SupplierInsertIfNotExistsCommand : BaseInsertIfNotExistsCommand<SupplierEntity>, ISupplierInsertIfNotExistsCommand
    {
        public SupplierInsertIfNotExistsCommand(IFindByIdQuery<SupplierEntity> findByIdQuery, IInsertCommand<SupplierEntity> insertCommand) : base(findByIdQuery, insertCommand) { }
    }
}
