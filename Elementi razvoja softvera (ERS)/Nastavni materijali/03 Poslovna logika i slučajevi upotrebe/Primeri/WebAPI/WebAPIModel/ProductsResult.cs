using Domain;
using System.Collections.Generic;

namespace WebAPI.WebAPIModel
{
    public class ProductsResult: BaseWebAPIResult
    {
        public List<ProductResult> Products { get; set; }
        
        public ProductsResult(List<ProductEntity> products)
        {
            products.ForEach(t => Products.Add(new ProductResult(t)));
        }

        protected override void InitProps()
        {
            Products = new List<ProductResult>();
        }
    }
}
