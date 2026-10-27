using Application.Customer.Commands;
using Application.Customer.Queries;
using Application.Order.Commands;
using Application.Order.Queries;
using Application.Product.Commands;
using Application.Product.Queries;
using Application.Supplier.Commands;
using Application.Supplier.Queries;
using Application.Stock.LocalStock.Commands;
using Application.Stock.LocalStock.Queries;
using Application.Common.Repo;
using Application.Stock.SupplierStock.Queries;
using Application.Stock.SupplierStock.Commands;

namespace Application.Common.Factory
{
    public interface IRepoFactory
    {
        IOrderInsertCommand OrderInsertCommand { get; }
        IOrderDeleteCommand OrderDeleteCommand { get; }
        IOrderInsertIfNotExistsCommand OrderInsertIfNotExistsCommand { get; }
        IOrderInsertOrUpdateCommand OrderInsertOrUpdateCommand { get; }
        IOrderUpdateCommand OrderUpdateCommand { get; }
        IOrderFindByIdQuery OrderFindByIdQuery { get; }
        IOrderGetListQuery OrderGetListQuery { get; }

        IProductInsertCommand ProductInsertCommand { get; }
        IProductDeleteCommand ProductDeleteCommand { get; }
        IProductInsertIfNotExistsCommand ProductInsertIfNotExistsCommand { get; }
        IProductInsertOrUpdateCommand ProductInsertOrUpdateCommand { get; }
        IProductUpdateCommand ProductUpdateCommand { get; }        
        IProductFindByIdQuery ProductFindByIdQuery { get; }
        IProductGetListQuery ProductGetListQuery { get; }

        ISupplierInsertCommand SupplierInsertCommand { get; }
        ISupplierDeleteCommand SupplierDeleteCommand { get; }
        ISupplierInsertIfNotExistsCommand SupplierInsertIfNotExistsCommand { get; }
        ISupplierInsertOrUpdateCommand SupplierInsertOrUpdateCommand { get; }
        ISupplierUpdateCommand SupplierUpdateCommand { get; }
        ISupplierFindByIdQuery SupplierFindByIdQuery { get; }
        ISupplierGetListQuery SupplierGetListQuery { get; }

        ICustomerInsertCommand CustomerInsertCommand { get; }
        ICustomerDeleteCommand CustomerDeleteCommand { get; }
        ICustomerInsertIfNotExistsCommand CustomerInsertIfNotExistsCommand { get; }
        ICustomerInsertOrUpdateCommand CustomerInsertOrUpdateCommand { get; }
        ICustomerUpdateCommand CustomerUpdateCommand { get; }
        ICustomerFindByIdQuery CustomerFindByIdQuery { get; }
        ICustomerGetListQuery CustomerGetListQuery { get; }

        ILocalStockInsertCommand LocalStockInsertCommand { get; }
        ILocalStockDeleteCommand LocalStockDeleteCommand { get; }
        ILocalStockInsertIfNotExistsCommand LocalStockInsertIfNotExistsCommand { get; }
        ILocalStockInsertOrUpdateCommand LocalStockInsertOrUpdateCommand { get; }
        ILocalStockUpdateCommand LocalStockUpdateCommand { get; }
        ILocalStockFindByIdQuery LocalStockFindByIdQuery { get; }
        ILocalStockGetListQuery LocalStockGetListQuery { get; }
        ILocalStockGetDefaultLocalStockQuery LocalStockGetDefaultLocalStockQuery { get; }

        ISupplierStockInsertCommand SupplierStockInsertCommand { get; }
        ISupplierStockDeleteCommand SupplierStockDeleteCommand { get; }
        ISupplierStockInsertIfNotExistsCommand SupplierStockInsertIfNotExistsCommand { get; }
        ISupplierStockInsertOrUpdateCommand SupplierStockInsertOrUpdateCommand { get; }
        ISupplierStockUpdateCommand SupplierStockUpdateCommand { get; }
        ISupplierStockFindByIdQuery SupplierStockFindByIdQuery { get; }
        ISupplierStockGetListQuery SupplierStockGetListQuery { get; }
        ISupplierStockGetDefaultSupplierStockQuery SupplierStockGetDefaultSupplierStockQuery { get; }

        IClearRepo CommonRepo { get; }
    }
}
