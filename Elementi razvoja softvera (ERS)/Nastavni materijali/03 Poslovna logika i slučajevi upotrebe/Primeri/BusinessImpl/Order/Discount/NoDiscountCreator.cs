using Domain;
using Application.Order.Interfaces;

namespace BusinessImpl.Order.Discount
{
    public class NoDiscountCreator : IDiscountCreator
    {
        public IDiscount NoDiscount { private get; set; }

        public NoDiscountCreator(IDiscount noDiscount) : base()
        {
            this.NoDiscount = noDiscount;
        }

        public IDiscount GetDiscount(OrderEntity order)
        {
            return NoDiscount;
        }
    }
}
