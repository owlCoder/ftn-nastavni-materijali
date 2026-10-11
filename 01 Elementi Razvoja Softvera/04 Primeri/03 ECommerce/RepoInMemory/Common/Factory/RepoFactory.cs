using Application.Common.Factory;
using Application.Order.Commands;
using Application.Order.Queries;
using Application.Product.Commands;
using Application.Product.Queries;
using Application.Supplier.Commands;
using Application.Supplier.Queries;
using Application.Stock.LocalStock.Commands;
using Application.Stock.LocalStock.Queries;
using RepoInMemory.Order.Commands;
using RepoInMemory.Order.Queries;
using RepoInMemory.Product.Commands;
using RepoInMemory.Product.Queries;
using RepoInMemory.Supplier.Commands;
using RepoInMemory.Supplier.Queries;
using RepoInMemory.Stock.LocalStock.Commands;
using RepoInMemory.Stock.LocalStock.Queries;
using RepoInMemory.Customer.Commands;
using RepoInMemory.Customer.Queries;
using Application.Customer.Commands;
using Application.Customer.Queries;
using Application.Common.Repo;
using Application.Stock.SupplierStock.Queries;
using Application.Stock.SupplierStock.Commands;
using RepoInMemory.Stock.SupplierStock.Commands;
using RepoInMemory.Stock.SupplierStock.Queries;
using RepoInMemory.Common.Repo;

namespace RepoInMemory.Common.Factory
{
    public class RepoFactory : IRepoFactory
    {

        private IOrderInsertCommand orderInsertCommand;
        private IOrderDeleteCommand orderDeleteCommand;
        private IOrderInsertIfNotExistsCommand orderInsertIfNotExistsCommand;
        private IOrderInsertOrUpdateCommand orderInsertOrUpdateCommand;
        private IOrderUpdateCommand orderUpdateCommand;
        private IOrderFindByIdQuery orderFindByIdQuery;
        private IOrderGetListQuery orderGetListQuery;

        private IProductInsertCommand productInsertCommand;
        private IProductDeleteCommand productDeleteCommand;
        private IProductInsertIfNotExistsCommand productInsertIfNotExistsCommand;
        private IProductInsertOrUpdateCommand productInsertOrUpdateCommand;
        private IProductUpdateCommand productUpdateCommand;
        private IProductFindByIdQuery productFindByIdQuery;
        private IProductGetListQuery productGetListQuery;

        private ISupplierInsertCommand supplierInsertCommand;
        private ISupplierDeleteCommand supplierDeleteCommand;
        private ISupplierInsertIfNotExistsCommand supplierInsertIfNotExistsCommand;
        private ISupplierInsertOrUpdateCommand supplierInsertOrUpdateCommand;
        private ISupplierUpdateCommand supplierUpdateCommand;
        private ISupplierFindByIdQuery supplierFindByIdQuery;
        private ISupplierGetListQuery supplierGetListQuery;

        private ICustomerInsertCommand customerInsertCommand;
        private ICustomerDeleteCommand customerDeleteCommand;
        private ICustomerInsertIfNotExistsCommand customerInsertIfNotExistsCommand;
        private ICustomerInsertOrUpdateCommand customerInsertOrUpdateCommand;
        private ICustomerUpdateCommand customerUpdateCommand;
        private ICustomerFindByIdQuery customerFindByIdQuery;
        private ICustomerGetListQuery customerGetListQuery;

        private ILocalStockInsertCommand localStockInsertCommand;
        private ILocalStockDeleteCommand localStockDeleteCommand;
        private ILocalStockInsertIfNotExistsCommand localStockInsertIfNotExistsCommand;
        private ILocalStockInsertOrUpdateCommand localStockInsertOrUpdateCommand;
        private ILocalStockUpdateCommand localStockUpdateCommand;
        private ILocalStockFindByIdQuery localStockFindByIdQuery;
        private ILocalStockGetListQuery localStockGetListQuery;
        private ILocalStockGetDefaultLocalStockQuery localStockGetDefaultLocalStockQuery;

        private ISupplierStockInsertCommand supplierStockInsertCommand;
        private ISupplierStockDeleteCommand supplierStockDeleteCommand;
        private ISupplierStockInsertIfNotExistsCommand supplierStockInsertIfNotExistsCommand;
        private ISupplierStockInsertOrUpdateCommand supplierStockInsertOrUpdateCommand;
        private ISupplierStockUpdateCommand supplierStockUpdateCommand;
        private ISupplierStockFindByIdQuery supplierStockFindByIdQuery;
        private ISupplierStockGetListQuery supplierStockGetListQuery;
        private ISupplierStockGetDefaultSupplierStockQuery supplierStockGetDefaultSupplierStockQuery;




        private IClearRepo commonRepo;

        private IOrderInsertCommand GetOrderInsertCommand()
        {
            if (orderInsertCommand == null)
            {
                orderInsertCommand = new OrderInsertCommand();
            }
            return orderInsertCommand;
        }
        private IOrderDeleteCommand GetOrderDeleteCommand()
        {
            if (orderDeleteCommand == null)
            {
                orderDeleteCommand = new OrderDeleteCommand();
            }
            return orderDeleteCommand;
        }
        private IOrderInsertIfNotExistsCommand GetOrderInsertIfNotExistsCommand()
        {
            if (orderInsertIfNotExistsCommand == null)
            {
                orderInsertIfNotExistsCommand = new OrderInsertIfNotExistsCommand(this.OrderFindByIdQuery, this.OrderInsertCommand);
            }
            return orderInsertIfNotExistsCommand;
        }
        private IOrderInsertOrUpdateCommand GetOrderInsertOrUpdateCommand()
        {
            if (orderInsertOrUpdateCommand == null)
            {
                orderInsertOrUpdateCommand = new OrderInsertOrUpdateCommand(this.OrderFindByIdQuery, this.OrderInsertCommand, this.OrderUpdateCommand);
            }
            return orderInsertOrUpdateCommand;
        }
        private IOrderUpdateCommand GetOrderUpdateCommand()
        {
            if (orderUpdateCommand == null)
            {
                orderUpdateCommand = new OrderUpdateCommand();
            }
            return orderUpdateCommand;
        }
        private IOrderFindByIdQuery GetOrderFindByIdQuery()
        {
            if (orderFindByIdQuery == null)
            {
                orderFindByIdQuery = new OrderFindByIdQuery();
            }
            return orderFindByIdQuery;
        }
        private IOrderGetListQuery GetOrderGetListQuery()
        {
            if (orderGetListQuery == null)
            {
                orderGetListQuery = new OrderGetListQuery();
            }
            return orderGetListQuery;
        }

        private IProductInsertCommand GetProductInsertCommand()
        {
            if (productInsertCommand == null)
            {
                productInsertCommand = new ProductInsertCommand();
            }
            return productInsertCommand;
        }
        private IProductDeleteCommand GetProductDeleteCommand()
        {
            if (productDeleteCommand == null)
            {
                productDeleteCommand = new ProductDeleteCommand();
            }
            return productDeleteCommand;
        }
        private IProductInsertIfNotExistsCommand GetProductInsertIfNotExistsCommand()
        {
            if (productInsertIfNotExistsCommand == null)
            {
                productInsertIfNotExistsCommand = new ProductInsertIfNotExistsCommand(this.ProductFindByIdQuery, this.ProductInsertCommand);
            }
            return productInsertIfNotExistsCommand;
        }
        private IProductInsertOrUpdateCommand GetProductInsertOrUpdateCommand()
        {
            if (productInsertOrUpdateCommand == null)
            {
                productInsertOrUpdateCommand = new ProductInsertOrUpdateCommand(this.ProductFindByIdQuery, this.ProductInsertCommand, this.ProductUpdateCommand);
            }
            return productInsertOrUpdateCommand;
        }
        private IProductUpdateCommand GetProductUpdateCommand()
        {
            if (productUpdateCommand == null)
            {
                productUpdateCommand = new ProductUpdateCommand();
            }
            return productUpdateCommand;
        }
        private IProductFindByIdQuery GetProductFindByIdQuery()
        {
            if (productFindByIdQuery == null)
            {
                productFindByIdQuery = new ProductFindByIdQuery();
            }
            return productFindByIdQuery;
        }
        private IProductGetListQuery GetProductGetListQuery()
        {
            if (productGetListQuery == null)
            {
                productGetListQuery = new ProductGetListQuery();
            }
            return productGetListQuery;
        }
        private ISupplierInsertCommand GetSupplierInsertCommand()
        {
            if (supplierInsertCommand == null)
            {
                supplierInsertCommand = new SupplierInsertCommand();
            }
            return supplierInsertCommand;
        }
        private ISupplierDeleteCommand GetSupplierDeleteCommand()
        {
            if (supplierDeleteCommand == null)
            {
                supplierDeleteCommand = new SupplierDeleteCommand();
            }
            return supplierDeleteCommand;
        }
        private ISupplierInsertIfNotExistsCommand GetSupplierInsertIfNotExistsCommand()
        {
            if (supplierInsertIfNotExistsCommand == null)
            {
                supplierInsertIfNotExistsCommand = new SupplierInsertIfNotExistsCommand(this.SupplierFindByIdQuery, this.SupplierInsertCommand);
            }
            return supplierInsertIfNotExistsCommand;
        }
        private ISupplierInsertOrUpdateCommand GetSupplierInsertOrUpdateCommand()
        {
            if (supplierInsertOrUpdateCommand == null)
            {
                supplierInsertOrUpdateCommand = new SupplierInsertOrUpdateCommand(this.SupplierFindByIdQuery, this.SupplierInsertCommand, this.SupplierUpdateCommand);
            }
            return supplierInsertOrUpdateCommand;
        }
        private ISupplierUpdateCommand GetSupplierUpdateCommand()
        {
            if (supplierUpdateCommand == null)
            {
                supplierUpdateCommand = new SupplierUpdateCommand();
            }
            return supplierUpdateCommand;
        }
        private ISupplierFindByIdQuery GetSupplierFindByIdQuery()
        {
            if (supplierFindByIdQuery == null)
            {
                supplierFindByIdQuery = new SupplierFindByIdQuery();
            }
            return supplierFindByIdQuery;
        }
        private ISupplierGetListQuery GetSupplierGetListQuery()
        {
            if (supplierGetListQuery == null)
            {
                supplierGetListQuery = new SupplierGetListQuery();
            }
            return supplierGetListQuery;
        }

        private ILocalStockInsertCommand GetLocalStockInsertCommand()
        {
            if (localStockInsertCommand == null)
            {
                localStockInsertCommand = new LocalStockInsertCommand();
            }
            return localStockInsertCommand;
        }
        private ILocalStockDeleteCommand GetLocalStockDeleteCommand()
        {
            if (localStockDeleteCommand == null)
            {
                localStockDeleteCommand = new LocalStockDeleteCommand();
            }
            return localStockDeleteCommand;
        }
        private ILocalStockInsertIfNotExistsCommand GetLocalStockInsertIfNotExistsCommand()
        {
            if (localStockInsertIfNotExistsCommand == null)
            {
                localStockInsertIfNotExistsCommand = new LocalStockInsertIfNotExistsCommand(this.LocalStockFindByIdQuery, this.LocalStockInsertCommand);
            }
            return localStockInsertIfNotExistsCommand;
        }
        private ILocalStockInsertOrUpdateCommand GetLocalStockInsertOrUpdateCommand()
        {
            if (localStockInsertOrUpdateCommand == null)
            {
                localStockInsertOrUpdateCommand = new LocalStockInsertOrUpdateCommand(this.LocalStockFindByIdQuery, this.LocalStockInsertCommand, this.LocalStockUpdateCommand);
            }
            return localStockInsertOrUpdateCommand;
        }
        private ILocalStockUpdateCommand GetLocalStockUpdateCommand()
        {
            if (localStockUpdateCommand == null)
            {
                localStockUpdateCommand = new LocalStockUpdateCommand();
            }
            return localStockUpdateCommand;
        }
        private ILocalStockFindByIdQuery GetLocalStockFindByIdQuery()
        {
            if (localStockFindByIdQuery == null)
            {
                localStockFindByIdQuery = new LocalStockFindByIdQuery();
            }
            return localStockFindByIdQuery;
        }
        private ILocalStockGetListQuery GetLocalStockGetListQuery()
        {
            if (localStockGetListQuery == null)
            {
                localStockGetListQuery = new LocalStockGetListQuery();
            }
            return localStockGetListQuery;
        }

        private ILocalStockGetDefaultLocalStockQuery GetLocalStockGetDefaultLocalStockQuery()
        {
            if (localStockGetDefaultLocalStockQuery == null)
            {
                localStockGetDefaultLocalStockQuery = new LocalStockGetDefaultLocalStockQuery();
            }
            return localStockGetDefaultLocalStockQuery;
        }

        private ISupplierStockInsertCommand GetSupplierStockInsertCommand()
        {
            if (supplierStockInsertCommand == null)
            {
                supplierStockInsertCommand = new SupplierStockInsertCommand();
            }
            return supplierStockInsertCommand;
        }
        private ISupplierStockDeleteCommand GetSupplierStockDeleteCommand()
        {
            if (supplierStockDeleteCommand == null)
            {
                supplierStockDeleteCommand = new SupplierStockDeleteCommand();
            }
            return supplierStockDeleteCommand;
        }
        private ISupplierStockInsertIfNotExistsCommand GetSupplierStockInsertIfNotExistsCommand()
        {
            if (supplierStockInsertIfNotExistsCommand == null)
            {
                supplierStockInsertIfNotExistsCommand = new SupplierStockInsertIfNotExistsCommand(this.SupplierStockFindByIdQuery, this.SupplierStockInsertCommand);
            }
            return supplierStockInsertIfNotExistsCommand;
        }
        private ISupplierStockInsertOrUpdateCommand GetSupplierStockInsertOrUpdateCommand()
        {
            if (supplierStockInsertOrUpdateCommand == null)
            {
                supplierStockInsertOrUpdateCommand = new SupplierStockInsertOrUpdateCommand(this.SupplierStockFindByIdQuery, this.SupplierStockInsertCommand, this.SupplierStockUpdateCommand);
            }
            return supplierStockInsertOrUpdateCommand;
        }
        private ISupplierStockUpdateCommand GetSupplierStockUpdateCommand()
        {
            if (supplierStockUpdateCommand == null)
            {
                supplierStockUpdateCommand = new SupplierStockUpdateCommand();
            }
            return supplierStockUpdateCommand;
        }
        private ISupplierStockFindByIdQuery GetSupplierStockFindByIdQuery()
        {
            if (supplierStockFindByIdQuery == null)
            {
                supplierStockFindByIdQuery = new SupplierStockFindByIdQuery();
            }
            return supplierStockFindByIdQuery;
        }
        private ISupplierStockGetListQuery GetSupplierStockGetListQuery()
        {
            if (supplierStockGetListQuery == null)
            {
                supplierStockGetListQuery = new SupplierStockGetListQuery();
            }
            return supplierStockGetListQuery;
        }

        private ISupplierStockGetDefaultSupplierStockQuery GetSupplierStockGetDefaultSupplierStockQuery()
        {
            if (supplierStockGetDefaultSupplierStockQuery == null)
            {
                supplierStockGetDefaultSupplierStockQuery = new SupplierStockGetDefaultSupplierStockQuery();
            }
            return supplierStockGetDefaultSupplierStockQuery;
        }


        private ICustomerInsertCommand GetCustomerInsertCommand()
        {
            if (customerInsertCommand == null)
            {
                customerInsertCommand = new CustomerInsertCommand();
            }
            return customerInsertCommand;
        }
        private ICustomerDeleteCommand GetCustomerDeleteCommand()
        {
            if (customerDeleteCommand == null)
            {
                customerDeleteCommand = new CustomerDeleteCommand();
            }
            return customerDeleteCommand;
        }
        private ICustomerInsertIfNotExistsCommand GetCustomerInsertIfNotExistsCommand()
        {
            if (customerInsertIfNotExistsCommand == null)
            {
                customerInsertIfNotExistsCommand = new CustomerInsertIfNotExistsCommand(this.CustomerFindByIdQuery, this.CustomerInsertCommand);
            }
            return customerInsertIfNotExistsCommand;
        }
        private ICustomerInsertOrUpdateCommand GetCustomerInsertOrUpdateCommand()
        {
            if (customerInsertOrUpdateCommand == null)
            {
                customerInsertOrUpdateCommand = new CustomerInsertOrUpdateCommand(this.CustomerFindByIdQuery, this.CustomerInsertCommand, this.CustomerUpdateCommand);
            }
            return customerInsertOrUpdateCommand;
        }
        private ICustomerUpdateCommand GetCustomerUpdateCommand()
        {
            if (customerUpdateCommand == null)
            {
                customerUpdateCommand = new CustomerUpdateCommand();
            }
            return customerUpdateCommand;
        }
        private ICustomerFindByIdQuery GetCustomerFindByIdQuery()
        {
            if (customerFindByIdQuery == null)
            {
                customerFindByIdQuery = new CustomerFindByIdQuery();
            }
            return customerFindByIdQuery;
        }
        private ICustomerGetListQuery GetCustomerGetListQuery()
        {
            if (customerGetListQuery == null)
            {
                customerGetListQuery = new CustomerGetListQuery();
            }
            return customerGetListQuery;
        }

        private IClearRepo GetCommonRepo()
        {
            if (commonRepo == null)
            {
                commonRepo = new ClearRepo();
            }
            return commonRepo;
        }

        public IOrderInsertCommand OrderInsertCommand => GetOrderInsertCommand();

        public IOrderDeleteCommand OrderDeleteCommand => GetOrderDeleteCommand();

        public IOrderInsertIfNotExistsCommand OrderInsertIfNotExistsCommand => GetOrderInsertIfNotExistsCommand();

        public IOrderInsertOrUpdateCommand OrderInsertOrUpdateCommand => GetOrderInsertOrUpdateCommand();

        public IOrderUpdateCommand OrderUpdateCommand => GetOrderUpdateCommand();

        public IOrderFindByIdQuery OrderFindByIdQuery => GetOrderFindByIdQuery();

        public IOrderGetListQuery OrderGetListQuery => GetOrderGetListQuery();

        public IProductInsertCommand ProductInsertCommand => GetProductInsertCommand();

        public IProductDeleteCommand ProductDeleteCommand => GetProductDeleteCommand();

        public IProductInsertIfNotExistsCommand ProductInsertIfNotExistsCommand => GetProductInsertIfNotExistsCommand();

        public IProductInsertOrUpdateCommand ProductInsertOrUpdateCommand => GetProductInsertOrUpdateCommand();

        public IProductUpdateCommand ProductUpdateCommand => GetProductUpdateCommand();

        public IProductFindByIdQuery ProductFindByIdQuery => GetProductFindByIdQuery();

        public IProductGetListQuery ProductGetListQuery => GetProductGetListQuery();

        public ISupplierInsertCommand SupplierInsertCommand => GetSupplierInsertCommand();

        public ISupplierDeleteCommand SupplierDeleteCommand => GetSupplierDeleteCommand();

        public ISupplierInsertIfNotExistsCommand SupplierInsertIfNotExistsCommand => GetSupplierInsertIfNotExistsCommand();

        public ISupplierInsertOrUpdateCommand SupplierInsertOrUpdateCommand => GetSupplierInsertOrUpdateCommand();

        public ISupplierUpdateCommand SupplierUpdateCommand => GetSupplierUpdateCommand();

        public ISupplierFindByIdQuery SupplierFindByIdQuery => GetSupplierFindByIdQuery();

        public ISupplierGetListQuery SupplierGetListQuery => GetSupplierGetListQuery();

        public ILocalStockInsertCommand LocalStockInsertCommand => GetLocalStockInsertCommand();

        public ILocalStockDeleteCommand LocalStockDeleteCommand => GetLocalStockDeleteCommand();

        public ILocalStockInsertIfNotExistsCommand LocalStockInsertIfNotExistsCommand => GetLocalStockInsertIfNotExistsCommand();

        public ILocalStockInsertOrUpdateCommand LocalStockInsertOrUpdateCommand => GetLocalStockInsertOrUpdateCommand();

        public ILocalStockUpdateCommand LocalStockUpdateCommand => GetLocalStockUpdateCommand();

        public ILocalStockFindByIdQuery LocalStockFindByIdQuery => GetLocalStockFindByIdQuery();

        public ILocalStockGetListQuery LocalStockGetListQuery => GetLocalStockGetListQuery();
        public ILocalStockGetDefaultLocalStockQuery LocalStockGetDefaultLocalStockQuery => GetLocalStockGetDefaultLocalStockQuery();

        public ICustomerInsertCommand CustomerInsertCommand => GetCustomerInsertCommand();

        public ICustomerDeleteCommand CustomerDeleteCommand => GetCustomerDeleteCommand();

        public ICustomerInsertIfNotExistsCommand CustomerInsertIfNotExistsCommand => GetCustomerInsertIfNotExistsCommand();

        public ICustomerInsertOrUpdateCommand CustomerInsertOrUpdateCommand => GetCustomerInsertOrUpdateCommand();

        public ICustomerUpdateCommand CustomerUpdateCommand => GetCustomerUpdateCommand();

        public ICustomerFindByIdQuery CustomerFindByIdQuery => GetCustomerFindByIdQuery();

        public ICustomerGetListQuery CustomerGetListQuery => GetCustomerGetListQuery();
        public ISupplierStockInsertCommand SupplierStockInsertCommand => GetSupplierStockInsertCommand();

        public ISupplierStockDeleteCommand SupplierStockDeleteCommand => GetSupplierStockDeleteCommand();

        public ISupplierStockInsertIfNotExistsCommand SupplierStockInsertIfNotExistsCommand => GetSupplierStockInsertIfNotExistsCommand();

        public ISupplierStockInsertOrUpdateCommand SupplierStockInsertOrUpdateCommand => GetSupplierStockInsertOrUpdateCommand();

        public ISupplierStockUpdateCommand SupplierStockUpdateCommand => GetSupplierStockUpdateCommand();

        public ISupplierStockFindByIdQuery SupplierStockFindByIdQuery => GetSupplierStockFindByIdQuery();

        public ISupplierStockGetListQuery SupplierStockGetListQuery => GetSupplierStockGetListQuery();
        public ISupplierStockGetDefaultSupplierStockQuery SupplierStockGetDefaultSupplierStockQuery => GetSupplierStockGetDefaultSupplierStockQuery();


        public IClearRepo CommonRepo => GetCommonRepo();
    }
}
