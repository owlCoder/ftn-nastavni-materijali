using Application.Customer.Queries;
using Domain;
using Domain.Results;
using System.Collections.Generic;

namespace Application.Order.Handlers
{
    public class ListCartContentHandler
    {
        public ICustomerFindByIdQuery CustomerFindByIdQuery { private get; set; }

        public ListCartContentHandler(ICustomerFindByIdQuery customerFindByIdQuery)
        {
            CustomerFindByIdQuery = customerFindByIdQuery;
        }
        public Result GetProductItems(string customerId)
        {
            Result result = new Result();
            CustomerEntity customer = CustomerFindByIdQuery.FindById(customerId);
            if (customer is null)
            {
                result.Log(LogLevel.Error, "Unknown customer");
            }
            List<ProductItem> productItems = customer.ShoppingCart;
            result.ResultObject = productItems;
            return result;
        }
    }
}
