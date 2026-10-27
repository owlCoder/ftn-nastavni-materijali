using Domain;
using Application.Stock.Interfaces;
using System.Linq;

namespace BusinessImpl.Stock
{
    public class LocalStockChecker : IStockChecker
    {
        public StockEntity stock;
        public LocalStockChecker()
        {
        }
        public float GetQuantity(ProductEntity product, StockEntity stock)
        {
            return stock.ProductStocks.FirstOrDefault(t => t.Product.ProductId == product.ProductId).Quantity;
        }
    }
}
