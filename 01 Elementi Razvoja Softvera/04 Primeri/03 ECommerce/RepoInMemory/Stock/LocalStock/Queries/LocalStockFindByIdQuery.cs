using Application.Stock.LocalStock.Queries;
using RepoInMemory.Common.Queries;
using Domain;

namespace RepoInMemory.Stock.LocalStock.Queries
{
    public class LocalStockFindByIdQuery : BaseFindByIdQuery<LocalStockEntity>, ILocalStockFindByIdQuery { }
}
