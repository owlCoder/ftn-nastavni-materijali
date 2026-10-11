using Domain;

namespace WebAPI.WebAPIModel
{
    public class ProductResult : BaseWebAPIResult
    {
        public ProductResult(ProductEntity product)
        {
            this.ProductName = product.ProductName;
            this.ProductPrice = product.ProductPrice.ToString();
            this.SupplierName = product.Supplier.SupplierName;
        }

        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public string SupplierName { get; set; }

        protected override void InitProps()
        {
            this.ProductName = string.Empty;
            this.ProductPrice = string.Empty;
            this.SupplierName = string.Empty;
        }
    }
}
