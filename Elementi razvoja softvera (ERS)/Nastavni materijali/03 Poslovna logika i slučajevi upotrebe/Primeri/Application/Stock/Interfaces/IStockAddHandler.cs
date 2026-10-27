using Domain;

namespace Application.Stock.Interfaces
{
    public interface IStockAddHandler
    {
        void AddProduct(ProductEntity product, StockEntity stock, float productQuantity);
    }
}
