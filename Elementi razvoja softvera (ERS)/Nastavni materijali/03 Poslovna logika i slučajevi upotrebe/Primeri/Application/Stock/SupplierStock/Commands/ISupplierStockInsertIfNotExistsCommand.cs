using Application.Common.Commands;
using Domain;

namespace Application.Stock.SupplierStock.Commands
{
    public interface ISupplierStockInsertIfNotExistsCommand: IInsertIfNotExistsCommand<SupplierStockEntity>
    {
    }
}
