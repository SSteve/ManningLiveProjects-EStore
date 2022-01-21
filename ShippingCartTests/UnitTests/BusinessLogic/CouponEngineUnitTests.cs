using ShoppingCartService.BusinessLogic;
using Xunit;

namespace ShippingCartTests.UnitTests
{
    public class CouponEngineUnitTests
    {
        [Fact]
        public void Return_zero_if_coupon_is_null()
        {
            var sut = new CouponEngine();

            var discount = sut.CalculateDiscount(null, null);

            Assert.Equal(0, discount);
        }
    }
}
