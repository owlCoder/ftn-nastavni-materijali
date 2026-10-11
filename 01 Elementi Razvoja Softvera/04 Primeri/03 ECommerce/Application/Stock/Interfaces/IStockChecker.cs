using Domain;

namespace Application.Stock.Interfaces
{
    public interface IStockChecker
    {
        float GetQuantity(ProductEntity product, StockEntity stock);
    }
}
