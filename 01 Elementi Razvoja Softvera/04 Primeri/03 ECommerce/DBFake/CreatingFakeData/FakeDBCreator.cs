using Application.Common.Factory;
using Application.Common.Repo;
using Application.Customer.Commands;
using Application.Customer.Queries;
using Application.Product.Commands;
using Application.Product.Queries;
using Application.Stock.LocalStock.Commands;
using Application.Stock.SupplierStock.Commands;
using Domain;
using System.Collections.Generic;

namespace DBFake.FakeDBCreator
{
    public class FakeDBCreator
    {
        private readonly ICustomerInsertCommand customerInsertCommand;
        private readonly ICustomerGetListQuery customerGetListQuery;
        private readonly IProductInsertCommand productInsertCommand;
        private readonly IProductGetListQuery productGetListQuery;
        private readonly ISupplierStockInsertCommand supplierStockInsertCommand;
        private readonly ILocalStockInsertCommand localStockInsertCommand;
        private readonly IClearRepo commonRepo;

        public FakeDBCreator(IRepoFactory repoFactory)
        {
            customerInsertCommand = repoFactory.CustomerInsertCommand;
            productGetListQuery = repoFactory.ProductGetListQuery;
            productInsertCommand = repoFactory.ProductInsertCommand;
            supplierStockInsertCommand = repoFactory.SupplierStockInsertCommand;
            localStockInsertCommand = repoFactory.LocalStockInsertCommand;
            customerGetListQuery = repoFactory.CustomerGetListQuery;
            commonRepo = repoFactory.CommonRepo;
            ProductQuantityOnStock = 100;
            ShoppingCartQuantity = 20;
            Price = 10;
            PhoneNumber = "+38165986321";
        }

        public float ProductQuantityOnStock { private get; set; }
        public float ShoppingCartQuantity { private get; set; }
        public float Price { private get; set; }
        public string PhoneNumber { private get; set; }

        public void InsertDBData()
        {
            commonRepo.ClearAll();
            SupplierEntity supplier = NewSupplier("1", "Nike");

            customerInsertCommand.Insert(NewCustomer("1", "John", "Smith", PhoneNumber, "New York", "The street", "2/4"));
            customerInsertCommand.Insert(NewCustomer("2", "Zoran", "Jankovic", PhoneNumber, "Novi Sad", "Toplice Milana", "1"));
            customerInsertCommand.Insert(NewCustomer("3", "Aleksandar", "Mitrovic", PhoneNumber, "London", "North street", "1"));

            productInsertCommand.Insert(NewProduct("1", "Shoes", Price, supplier));
            productInsertCommand.Insert(NewProduct("2", "Sweat Suit", Price, supplier));
            productInsertCommand.Insert(NewProduct("3", "T-Shirt", Price, supplier));

            LocalStockEntity localStock = NewLocalStock(1, "Main stock", "1st street 3", true);
            List<ProductEntity> allProducts = productGetListQuery.GetList();
            allProducts.ForEach(t => localStock.ProductStocks.Add(new ProductStock(t, localStock, ProductQuantityOnStock)));
            localStockInsertCommand.Insert(localStock);
            
            SupplierStockEntity supplierStock = NewSupplierStock(1, "Nike B2B", "http:\\nike?product", true);
            supplierStock.Supplier = supplier;
            supplierStockInsertCommand.Insert(supplierStock);
        }

        public void InsertShoppingCarts()
        {
            List<ProductEntity> products = productGetListQuery.GetList();
            List<CustomerEntity> customers = customerGetListQuery.GetList();
            customers.ForEach(t1 => products.ForEach(t2 => t1.ShoppingCart.Add(new ProductItem(t2, ShoppingCartQuantity))));
        }

        private CustomerEntity NewCustomer(string customerId, string firstName, string lastName, string phoneNumber, string city, string street, string houseNumber)
        {
            CustomerEntity cust = new CustomerEntity
            {
                CustomerId = customerId
            };
            cust.FirstName = firstName;
            cust.LastName = lastName;
            cust.PhoneNumber = phoneNumber;
            cust.City = city;
            cust.Street = street;
            cust.HouseNumber = houseNumber;
            return cust;
        }

        private ProductEntity NewProduct(string productId, string productName, float productPrice, SupplierEntity supplier)
        {
            ProductEntity product = new ProductEntity
            {
                ProductId = productId,
                ProductName = productName,
                ProductPrice = productPrice,
                Supplier = supplier
            };
            return product;
        }

        private LocalStockEntity NewLocalStock(int stockId, string stockName, string address, bool isDefault)
        {
            LocalStockEntity localStock = new LocalStockEntity
            {
                Address = address,
                IsDefault = isDefault,
                StockId = stockId,
                StockName = stockName
            };
            return localStock;
        }

        private SupplierStockEntity NewSupplierStock(int stockId, string stockName, string url, bool isDefault)
        {
            SupplierStockEntity supplierStock = new SupplierStockEntity
            {
                IsDefault = isDefault,
                StockId = stockId,
                StockName = stockName,
                WebServiceURL = url
            };
            return supplierStock;
        }

        private SupplierEntity NewSupplier(string supplierId, string supplierName)
        {
            SupplierEntity supplier = new SupplierEntity
            {
                SupplierId = supplierId,
                SupplierName = supplierName
            };
            return supplier;
        }
    }
}
