using Domain.Exceptions;
using System;

namespace Domain.Helpers
{
    public class EmptyEntityBuilder
    {
        private static readonly Lazy<EmptyEntityBuilder> instance = new Lazy<EmptyEntityBuilder>(() => new EmptyEntityBuilder());
        public static EmptyEntityBuilder Instance { get { return instance.Value; } }
        private EmptyEntityBuilder() { }

        public T GetEmptyEntity<T>() where T : Entity
        {
            var tType = typeof(T);
            if (tType == typeof(CustomerEntity))
            {
                return new CustomerEntity() as T;
            }
            else if (tType == typeof(OrderEntity))
            {
                return new OrderEntity() as T;
            }
            else if (tType == typeof(ProductEntity))
            {
                return new ProductEntity() as T;
            }
            else if (tType == typeof(ShoppingCartEntity))
            {
                return new ShoppingCartEntity() as T;
            }
            else if (tType == typeof(LocalStockEntity))
            {
                return new LocalStockEntity() as T;
            }
            else if (tType == typeof(SupplierStockEntity))
            {
                return new SupplierStockEntity() as T;
            }
            throw new UnknownModelException();
        }
    }
}
