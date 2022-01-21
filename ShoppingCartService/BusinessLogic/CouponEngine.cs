using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess.Entities;

namespace ShoppingCartService.BusinessLogic
{
    public class CouponEngine
    {
        public CouponEngine()
        {
        }

        public double CalculateDiscount(CheckoutDto checkoutDto, Coupon coupon)
        {
            return -1;
        }
    }
}
