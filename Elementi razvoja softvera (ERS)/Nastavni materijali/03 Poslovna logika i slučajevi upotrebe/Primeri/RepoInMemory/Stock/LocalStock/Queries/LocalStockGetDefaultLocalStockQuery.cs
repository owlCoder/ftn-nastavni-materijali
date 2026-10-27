using Application.Stock.LocalStock.Queries;
using Domain;
using RepoInMemory.Common.DB;
using System.Linq;

namespace RepoInMemory.Stock.LocalStock.Queries
{
    public class LocalStockGetDefaultLocalStockQuery : ILocalStockGetDefaultLocalStockQuery
    {
        public LocalStockEntity GetDefaultLocalStock()
        {
            return InMemoryDatabase.Instance.LocalStocks.FirstOrDefault(t => t.Value.IsDefault).Value;
        }
    }
}
