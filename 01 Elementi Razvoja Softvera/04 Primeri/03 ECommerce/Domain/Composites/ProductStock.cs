namespace Domain
{
    public class ProductStock
    {
        public ProductStock(ProductEntity product, StockEntity stock, float quantity)
        {
            this.Product = product;
            this.Stock = stock;
            this.Quantity = quantity;
        }

        public ProductEntity Product { get; set; }
        public StockEntity Stock { get; set; }
        public float Quantity { get; set; }
    }
}
