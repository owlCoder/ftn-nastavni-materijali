using Application.Customer.Queries;
using Application.Order.Interfaces;

namespace BusinessImpl.Order.Discount
{
    public class PhoneNumberEndDiscount : IDiscount
    {
        public ICustomerFindByIdQuery CustomerFindByIdQuery { get; set; }
        private string customerId;
        public PhoneNumberEndDiscount(ICustomerFindByIdQuery customerFindByIdQuery, string customerId)
        {
            this.CustomerFindByIdQuery = customerFindByIdQuery;
            this.customerId = customerId;            
        }
        public override float GetDiscountPercentage()
        {
            var customer = this.CustomerFindByIdQuery.FindById(customerId);
            string sPhoneNumberEnd = PhoneNumberEnd(customer.PhoneNumber);
            if (!int.TryParse(sPhoneNumberEnd, out int nPhoneNumberEnd))
            {
                return 0;
            }
            if (nPhoneNumberEnd == 0)
            {
                return 30;
            }
            if (nPhoneNumberEnd % 2 == 0)
            {
                return 20;
            }
            else
            {
                return 10;
            }
        }

        private string PhoneNumberEnd(string phoneNumber)
        {
            return phoneNumber[phoneNumber.Length - 1].ToString();
        }
    }
}
