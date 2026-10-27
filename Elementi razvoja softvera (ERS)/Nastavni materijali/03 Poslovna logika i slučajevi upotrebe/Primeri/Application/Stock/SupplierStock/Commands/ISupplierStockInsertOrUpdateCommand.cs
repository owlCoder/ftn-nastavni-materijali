using Application.Common.Commands;
using Domain;

namespace Application.Stock.SupplierStock.Commands
{
    public interface ISupplierStockInsertOrUpdateCommand: IInsertOrUpdateCommand<SupplierStockEntity>
    {
    }
}
