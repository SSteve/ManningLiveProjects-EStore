using System;
using ShoppingCartService.DataAccess.Entities;

namespace ShippingCartTests.Builders
{
    public static class CouponBuilder
    {
        public static Coupon BuildCoupon(
            string id = "",
            CouponType couponType = CouponType.Absolute,
            double couponValue = 0,
            DateTime? expirationDate = null)
        {
            if (!expirationDate.HasValue)
            {
                expirationDate = DateTime.MaxValue;
            }
            var coupon = new Coupon
            {
                Id = id,
                Type = couponType,
                Value = couponValue,
                ExpirationDate = expirationDate.Value,
            };
            return coupon;
        }
    }
}
