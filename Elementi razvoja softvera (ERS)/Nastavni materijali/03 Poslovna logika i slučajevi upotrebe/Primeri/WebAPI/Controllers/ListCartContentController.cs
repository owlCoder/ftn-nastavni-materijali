using System.Collections.Generic;
using Domain;
using Domain.Results;
using WebAPI.WebAPIModel;
using Microsoft.AspNetCore.Mvc;
using Application.Order.Handlers;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListCartContentController : BaseController
    {
        [HttpGet("{customerId}", Name = "ListCartContentControllerGet")]
        public ProductItemsResult Get(string customerId)
        {
            var listCartContentHandler = new ListCartContentHandler(repoFactory.CustomerFindByIdQuery);
            Result productItemResult = listCartContentHandler.GetProductItems(customerId);
            object resultObject = productItemResult.ResultObject;
            if (resultObject is null || !(resultObject is List<ProductItem>))
            {
                return new ProductItemsResult("Unknown error");
            }
            return new ProductItemsResult(resultObject as List<ProductItem>);
        }
    }
}
