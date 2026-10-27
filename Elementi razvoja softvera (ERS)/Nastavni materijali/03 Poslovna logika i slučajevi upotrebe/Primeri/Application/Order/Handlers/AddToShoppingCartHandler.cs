using Domain;
using Domain.Results;
using Application.Stock.Interfaces;
using Application.Supplier.Interfaces;
using Application.Common.Factory;
using Application.Customer.Queries;
using Application.Customer.Commands;
using Application.Product.Queries;
using Application.Stock.LocalStock.Queries;
using Application.Stock.SupplierStock.Queries;

namespace Application.Order.Handlers
{
    public class AddToShoppingCartHandler
    {

        public ICustomerFindByIdQuery CustomerFindByIdQuery { private get; set; }
        public ICustomerUpdateCommand CustomerUpdateCommand { private get; set; }
        public IProductFindByIdQuery ProductFindByIdQuery { private get; set; }
        public ILocalStockGetDefaultLocalStockQuery StockGetDefaultLocalStockQuery { private get; set; }
        public ISupplierStockGetDefaultSupplierStockQuery SupplierStockGetDefaultSupplierStockQuery { private get; set; }
        public ISupplierStockService SupplierStockService { private get; set; }

        public IStockWithdrawHandler CommonStockWithdrawHandler { private get; set; }
        public IStockAvailabilityChecker StockAvailabilityChecker { private get; set; }

        public AddToShoppingCartHandler(
            IRepoFactory repoFactory,
            ISupplierStockService supplierStockService,
            IStockWithdrawHandler commonStockWithdrawHandler,
            IStockAvailabilityChecker stockAvailabilityChecker
            ) : base()
        {
            this.CustomerFindByIdQuery = repoFactory.CustomerFindByIdQuery;
            this.CustomerUpdateCommand = repoFactory.CustomerUpdateCommand;
            this.ProductFindByIdQuery = repoFactory.ProductFindByIdQuery;
            this.StockGetDefaultLocalStockQuery = repoFactory.LocalStockGetDefaultLocalStockQuery;
            this.SupplierStockGetDefaultSupplierStockQuery = repoFactory.SupplierStockGetDefaultSupplierStockQuery;
            this.CommonStockWithdrawHandler = commonStockWithdrawHandler;
            this.StockAvailabilityChecker = stockAvailabilityChecker;
        }

        public Result AddToShoppingCart(string productId, string customerId, float quantity)
        {
            Result result = new Result();
            var customer = CustomerFindByIdQuery.FindById(customerId);
            if (customer is null)
            {
                result.Log(LogLevel.Error, $"Unknown customer");
                return result;
            }
            ProductEntity product = ProductFindByIdQuery.FindById(productId);
            SupplierEntity supplier = product.Supplier;
            SupplierStockEntity supplierStock = SupplierStockGetDefaultSupplierStockQuery.GetDefaultSupplierStock(supplier);
            var localStock = StockGetDefaultLocalStockQuery.GetDefaultLocalStock();
            if (StockAvailabilityChecker.CheckAvailability(product, localStock, supplierStock, quantity, result))
            {
                CommonStockWithdrawHandler.WithdrawProduct(product, quantity, localStock, supplierStock, out float missingQuantity, result);
                if (missingQuantity > 0)
                {
                    result.Log(LogLevel.Error, $"Missing quantity for the product: {missingQuantity}");
                }
                if (result.Success)
                {
                    customer.ShoppingCart.Add(new ProductItem(product, quantity));
                    CustomerUpdateCommand.Update(customer);
                }
            }
            result.ResultObject = customer.ShoppingCart;
            return result;
        }
    }
}
