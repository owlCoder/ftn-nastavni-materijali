namespace Domain
{
    public class ProductItem
    {
        public ProductItem(ProductEntity product, float quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
            this.UnitPrice = product.ProductPrice;
        }


        public ProductEntity Product { get; set; }
        public float Quantity { get; set; }
        public float UnitPrice { get; set; }
    }
}
