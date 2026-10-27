using Application.Common.Commands;
using Application.Common.Queries;
using Application.Supplier.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Supplier.Commands
{
    public class SupplierInsertOrUpdateCommand : BaseInsertOrUpdateCommand<SupplierEntity>, ISupplierInsertOrUpdateCommand
    {
        public SupplierInsertOrUpdateCommand(IFindByIdQuery<SupplierEntity> findByIdQuery, IInsertCommand<SupplierEntity> insertCommand, IUpdateCommand<SupplierEntity> updateCommand) : base(findByIdQuery, insertCommand, updateCommand) { }
    }
}
