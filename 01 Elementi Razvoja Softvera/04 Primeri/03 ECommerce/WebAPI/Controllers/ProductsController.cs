using Application.Product.Handlers;
using WebAPI.WebAPIModel;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        [HttpGet]
        public ProductsResult Get()
        {
            var productListingHandler = new ProductListingHandler(repoFactory.ProductGetListQuery);
            return new ProductsResult(productListingHandler.GetAllProducts());
        }
    }
}
