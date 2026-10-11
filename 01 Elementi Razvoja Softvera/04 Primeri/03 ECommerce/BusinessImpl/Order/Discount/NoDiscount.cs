using Application.Order.Interfaces;

namespace BusinessImpl.Order.Discount
{
    public class NoDiscount : IDiscount
    {
        public override float GetDiscountPercentage()
        {
            return 0;
        }
    }
}
