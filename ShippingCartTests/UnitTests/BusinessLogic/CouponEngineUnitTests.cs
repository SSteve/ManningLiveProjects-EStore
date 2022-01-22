using System;
using ShippingCartTests.Builders;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.BusinessLogic.Exceptions;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess.Entities;
using Xunit;

namespace ShippingCartTests.UnitTests
{
    public class CouponEngineUnitTests
    {
        [Fact]
        public void Return_zero_if_coupon_is_null()
        {
            var sut = new CouponEngine();
            var checkoutDto = BuildCheckoutDto();
            var discount = sut.CalculateDiscount(checkoutDto, null);

            Assert.Equal(0, discount);
        }

        [InlineData(10, 10)]
        [InlineData(1, 1)]
        [InlineData(3.33, 3.33)]
        [Theory]
        public void Return_correct_coupon_value(double couponValue, double cartTotal)
        {
            var coupon = CouponBuilder.BuildCoupon(couponValue: couponValue);
            var sut = new CouponEngine();

            var checkoutDto = BuildCheckoutDto(cartTotal);
            var value = sut.CalculateDiscount(checkoutDto, coupon);

            Assert.Equal(couponValue, value);
        }

        [Fact]
        public void Coupon_value_cant_be_greater_than_total()
        {
            const double couponValue = 30.0;
            const double cartTotal = 10.0;
            var coupon = CouponBuilder.BuildCoupon(couponValue: couponValue);
            var checkoutDto = BuildCheckoutDto(cartTotal);
            var sut = new CouponEngine();

            Assert.Throws<InvalidCouponException>(() => sut.CalculateDiscount(checkoutDto, coupon));
        }

        [Fact]
        public void Coupon_value_cant_be_negative()
        {
            const double couponValue = -1;
            var coupon = CouponBuilder.BuildCoupon(couponValue: couponValue);
            var checkoutDto = BuildCheckoutDto();
            var sut = new CouponEngine();

            Assert.Throws<InvalidCouponException>(() => sut.CalculateDiscount(checkoutDto, coupon));
        }

        [Fact]
        public void Percentage_coupon_calculates_correctly()
        {
            const double couponValue = 12.5;
            const double cartTotal = 166.85;
            var coupon = CouponBuilder.BuildCoupon(couponType: CouponType.Percentage, couponValue: couponValue);
            var checkoutDto = BuildCheckoutDto(total: cartTotal);
            var sut = new CouponEngine();
            var expected = Math.Round(cartTotal * couponValue / 100.0, 2);

            var discount = Math.Round(sut.CalculateDiscount(checkoutDto, coupon), 2);

            Assert.Equal(expected, discount);
        }

        [Fact]
        public void Percentage_coupon_must_be_less_than_100()
        {
            const double couponValue = 112.5;
            var coupon = CouponBuilder.BuildCoupon(couponType: CouponType.Percentage, couponValue: couponValue);
            var checkoutDto = BuildCheckoutDto();
            var sut = new CouponEngine();

            Assert.Throws<InvalidCouponException>(() => sut.CalculateDiscount(checkoutDto, coupon));
        }

        [Fact]
        public void Free_shipping_coupon_has_correct_value()
        {
            const double shippingCost = 18.95;
            var coupon = CouponBuilder.BuildCoupon(couponType: CouponType.FreeShipping);
            var checkoutDto = BuildCheckoutDto(shippingCost: shippingCost);
            var sut = new CouponEngine();

            var discount = Math.Round(sut.CalculateDiscount(checkoutDto, coupon), 2);

            Assert.Equal(shippingCost, discount);
        }

        [Fact]
        public void Expired_coupon_throws_exception()
        {
            var today = DateTime.Now;
            var couponDate = today.AddDays(-1);
            var coupon = CouponBuilder.BuildCoupon(expirationDate: couponDate);
            var checkoutDto = BuildCheckoutDto();
            var sut = new CouponEngine();

            Assert.Throws<CouponExpiredException>(() => sut.CalculateDiscount(checkoutDto, coupon));
        }

        private CheckoutDto BuildCheckoutDto(double total = 0, double shippingCost = 0)
        {
            var checkoutDto = new CheckoutDto(
                null, shippingCost, 0, total);
            return checkoutDto;
        }
    }
}
