using Domain;
using Domain.Exceptions;
using System;
using System.Collections.Concurrent;

namespace RepoInMemory.Common.DB
{
    public class InMemoryDatabase
    {


        private static readonly Lazy<InMemoryDatabase> instance = new Lazy<InMemoryDatabase>(() => new InMemoryDatabase());
        public static InMemoryDatabase Instance { get { return instance.Value; } }

        public ConcurrentDictionary<object, CustomerEntity> Customers { get; private set; }
        public ConcurrentDictionary<object, OrderEntity> Orders { get; private set; }
        public ConcurrentDictionary<object, ProductEntity> Products { get; private set; }
        public ConcurrentDictionary<object, ShoppingCartEntity> ShoppingCarts { get; private set; }
        public ConcurrentDictionary<object, LocalStockEntity> LocalStocks { get; private set; }
        public ConcurrentDictionary<object, SupplierStockEntity> SupplierStocks { get; private set; }

        private InMemoryDatabase()
        {
            Customers = new ConcurrentDictionary<object, CustomerEntity>();
            Orders = new ConcurrentDictionary<object, OrderEntity>();
            Products = new ConcurrentDictionary<object, ProductEntity>();
            ShoppingCarts = new ConcurrentDictionary<object, ShoppingCartEntity>();
            LocalStocks = new ConcurrentDictionary<object, LocalStockEntity>();
            SupplierStocks = new ConcurrentDictionary<object, SupplierStockEntity>();
        }

        public ConcurrentDictionary<object, TEntity> Set<TEntity>() where TEntity : Entity
        {
            var typeOfEntity = typeof(TEntity);
            if (typeOfEntity == typeof(CustomerEntity))
            {
                return Customers as ConcurrentDictionary<object, TEntity>;
            }
            else if (typeOfEntity == typeof(OrderEntity))
            {
                return Orders as ConcurrentDictionary<object, TEntity>;
            }
            else if (typeOfEntity == typeof(ProductEntity))
            {
                return Products as ConcurrentDictionary<object, TEntity>;
            }
            else if (typeOfEntity == typeof(ShoppingCartEntity))
            {
                return ShoppingCarts as ConcurrentDictionary<object, TEntity>;
            }
            else if (typeOfEntity == typeof(LocalStockEntity))
            {
                return LocalStocks as ConcurrentDictionary<object, TEntity>;
            }
            else if (typeOfEntity == typeof(SupplierStockEntity))
            {
                return SupplierStocks as ConcurrentDictionary<object, TEntity>;
            }
            throw new UnknownModelException();
        }

        public void Clear()
        {
            Customers.Clear();
            Orders.Clear();
            Products.Clear();
            ShoppingCarts.Clear();
            LocalStocks.Clear();
            SupplierStocks.Clear();
        }
    }
}
