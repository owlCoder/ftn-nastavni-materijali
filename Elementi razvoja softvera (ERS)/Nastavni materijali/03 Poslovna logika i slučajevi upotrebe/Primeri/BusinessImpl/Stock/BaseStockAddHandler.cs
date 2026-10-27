using Domain;
using Application.Stock.Interfaces;
using System.Linq;

namespace BusinessImpl.Stock
{
    public abstract class BaseStockAddHandler
    {
        protected StockEntity stock;
        protected IStockChecker stockChecker;
        public BaseStockAddHandler() { }

        protected void AddProductUpdateStock(ProductEntity product, StockEntity stock, float productQuntity)
        {
            ProductStock ps = stock.ProductStocks.Where(t => t.Product.ProductId == product.ProductId && t.Stock.StockId == stock.StockId).FirstOrDefault();
            if (ps != null)
            {
                ps.Quantity += productQuntity;
            }
            else
            {
                ps = new ProductStock(product, stock, productQuntity);
                stock.ProductStocks.Add(ps);
            }
        }
    }
}
