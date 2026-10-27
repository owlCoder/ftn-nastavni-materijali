using Application.Common.Queries;
using Domain;

namespace Application.Stock.LocalStock.Queries
{
    public interface ILocalStockFindByIdQuery : IFindByIdQuery<LocalStockEntity>
    {
    }
}
