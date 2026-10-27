using Application.Common.Commands;
using Domain;

namespace Application.Supplier.Commands
{
    public interface ISupplierInsertOrUpdateCommand: IInsertOrUpdateCommand<SupplierEntity>
    {
    }
}
