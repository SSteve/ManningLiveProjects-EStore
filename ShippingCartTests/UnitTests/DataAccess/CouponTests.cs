using ShippingCartTests.Builders;
using Xunit;
namespace ShippingCartTests.UnitTests
{
    public class CouponTests
    {
        [Fact]
        public void Same_coupons_are_equal()
        {
            var coupon1 = CouponBuilder.BuildCoupon(id: "PRG-001");
            var coupon2 = CouponBuilder.BuildCoupon(id: "PRG-001");

            Assert.Equal(coupon1, coupon2);
        }

        [Fact]
        public void Equal_coupons_have_same_hash_code()
        {
            var coupon1 = CouponBuilder.BuildCoupon(id: "PRG-001");
            var coupon2 = CouponBuilder.BuildCoupon(id: "PRG-001");

            Assert.Equal(coupon1.GetHashCode(), coupon2.GetHashCode());
        }

        [Fact]
        public void Different_coupons_are_not_equal()
        {
            var coupon1 = CouponBuilder.BuildCoupon(id: "PRG-001");
            var coupon2 = CouponBuilder.BuildCoupon(id: "PRG-002");

            Assert.NotEqual(coupon1, coupon2);
        }
    }
}
