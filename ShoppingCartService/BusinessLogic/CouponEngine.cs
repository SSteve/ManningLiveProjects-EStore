using ShoppingCartService.BusinessLogic.Exceptions;
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
            if (coupon is null)
                return 0.0;

            if (coupon.ExpirationDate < System.DateTime.Now)
                throw new CouponExpiredException();

            if (coupon.Value < 0)
                throw new InvalidCouponException();

            if (coupon.Type == CouponType.Absolute && coupon.Value > checkoutDto.Total)
                throw new InvalidCouponException();

            if (coupon.Type == CouponType.Percentage && coupon.Value > 100.0)
                throw new InvalidCouponException();

            switch (coupon.Type)
            {
                case CouponType.Percentage:
                    return checkoutDto.Total * coupon.Value / 100.0;
                case CouponType.Absolute:
                    return coupon.Value;
                case CouponType.FreeShipping:
                    return checkoutDto.ShippingCost;
                default:
                    throw new InvalidCouponException();
            }
        }
    }
}
