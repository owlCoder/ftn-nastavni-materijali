using BusinessImpl.Stock;
using Domain;
using Domain.Results;
using WebAPI.Constants;
using WebAPI.WebAPIModel;
using Application.Stock.Interfaces;
using Application.Supplier.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SupplierWebService;
using Application.Order.Handlers;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddProductToCartController : BaseController
    {
        public ISupplierStockService SupplierStockService { private get; set; }
        public IStockWithdrawHandler StockWithdrawHandler { private get; set; }
        public IStockAvailabilityChecker StockAvailabilityChecker { private get; set; }
        public IStockChecker LocalStockChecker { private get; set; }
        public IStockChecker SupplierStockChecker { private get; set; }


        public AddProductToCartController() : base()
        {
            this.SupplierStockService = new SupplierStockService();
            this.LocalStockChecker = new LocalStockChecker();
            InitializeApp();
        }

        public void InitializeApp()
        {
            this.SupplierStockChecker = new SupplierStockChecker(SupplierStockService);
            this.StockWithdrawHandler = new CommonStockWithdrawHandler(
                new LocalStockWithdrawHandler(repoFactory),
                new SupplierStockWithdrawHandler(SupplierStockService),
                new LocalStockAddHandler(repoFactory),
                new SupplierStockAddHandler(SupplierStockService)
                );
            this.StockAvailabilityChecker = new StockAvailabilityChecker(LocalStockChecker, SupplierStockChecker);
        }

        [HttpGet("{customerId}/{productId}/{quantity}", Name = "GetAddProductToCart")]
        public ProductItemsResult Get(string customerId, string productId, float quantity)
        {
            var addToShoppingCartHandler = new AddToShoppingCartHandler(
                repoFactory,
                SupplierStockService,
                StockWithdrawHandler,
                StockAvailabilityChecker
                );
            Result result = addToShoppingCartHandler.AddToShoppingCart(productId, customerId, quantity);
            if (result == null || (result.Success && !(result.ResultObject is ShoppingCartEntity)))
            {
                return new ProductItemsResult($"Adding product not succesful. Unknown error occured.");
            }
            if (!result.Success)
            {
                var rst = new ProductItemsResult($"Adding product not succesful. {result.GetErrorMessage()}");
                if (result.ResultObject != null && result.ResultObject is ShoppingCartEntity)
                {
                    rst.ApplyProductItems((ShoppingCartEntity)result.ResultObject);
                }
                return rst;
            }
            var rslt = new ProductItemsResult((ShoppingCartEntity)(result.ResultObject))
            {
                Message = MessageConstants.ADDING_PRODUCT_TO_SHOPPING_CART_SUCCESFUL
            };
            return rslt;
        }
    }
}
