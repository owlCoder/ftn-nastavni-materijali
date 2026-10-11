using Application.Common.Commands;
using Domain;

namespace Application.Stock.LocalStock.Commands
{
    public interface ILocalStockInsertOrUpdateCommand: IInsertOrUpdateCommand<LocalStockEntity>
    {
    }
}
