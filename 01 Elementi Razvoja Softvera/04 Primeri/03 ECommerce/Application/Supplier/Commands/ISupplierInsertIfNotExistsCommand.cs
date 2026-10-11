using Application.Common.Commands;
using Domain;

namespace Application.Supplier.Commands
{
    public interface ISupplierInsertIfNotExistsCommand: IInsertIfNotExistsCommand<SupplierEntity>
    {
    }
}
