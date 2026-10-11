using Application.Product.Queries;
using Domain;
using System.Collections.Generic;

namespace Application.Product.Handlers
{
    public class ProductListingHandler
    {
        public IProductGetListQuery ProductGetListQuery { private get; set; }
        
        public ProductListingHandler(IProductGetListQuery productGetListQuery)
        {
            ProductGetListQuery = productGetListQuery;
        }
        public List<ProductEntity> GetAllProducts()
        {
            return ProductGetListQuery.GetList();
        }
    }
}
