using System.Collections.Generic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using Xunit;

namespace ShippingCartTests.UnitTests
{
    /// <summary>
    /// The Cart class defines Equals and GetHashCode which need to be tested.
    /// </summary>
    public class CartTests
    {
        [Fact]
        public void Same_carts_are_equal()
        {
            var cart1 = BuildCart();
            var cart2 = BuildCart();

            var cartsAreEqual = Equals(cart1, cart2);

            Assert.True(cartsAreEqual);
        }

        [Fact]
        public void Different_carts_are_not_equal()
        {
            var cart1 = BuildCart();
            var cart2 = BuildCart();
            cart2.CustomerId = "A different customer id";

            var cartsAreEqual = Equals(cart1, cart2);

            Assert.False(cartsAreEqual);
        }

        /// <summary>
        /// Note: this test fails. I think this uncovered a bug in Cart.GetHashCode().
        /// According to .Net documentaion (https://docs.microsoft.com/en-us/dotnet/api/system.object.gethashcode?view=net-5.0)
        /// "Two objects that are equal return hash codes that are equal."
        /// </summary>
        [Fact]
        public void Same_carts_have_same_hashcode()
        {
            var cart1 = BuildCart();
            var cart2 = BuildCart();

            var cart1HashCode = cart1.GetHashCode();
            var cart2HashCode = cart2.GetHashCode();

            Assert.Equal(cart1HashCode, cart2HashCode);
        }

        [Fact]
        public void Different_carts_have_different_hashcodes()
        {
            var cart1 = BuildCart();
            var cart2 = BuildCart();
            cart2.CustomerId = "A different customer id";

            var cart1HashCode = cart1.GetHashCode();
            var cart2HashCode = cart2.GetHashCode();

            Assert.NotEqual(cart1HashCode, cart2HashCode);
        }

        /// <summary>
        /// Build a cart with test values.
        /// </summary>
        /// <returns></returns>
        private Cart BuildCart()
        {
            var cartId = "cart83748";
            var customerId = "ifjj3223";
            var customerType = CustomerType.Premium;
            var shippingMethod = ShippingMethod.Express;
            var address = new Address
            {
                Street = "Street",
                City = "City",
                Country = "Country",
            };
            var items = new List<Item>
            {
                new Item
                {
                    ProductId = "jfjf",
                    Quantity = 884,
                    Price = 33.33,
                    ProductName = "A thing to buy"
                },
                new Item
                {
                    ProductId = "fe54",
                    Quantity = 4,
                    Price = 44.44,
                    ProductName = "Another thing to buy"
                },
            };
            var cart = new Cart
            {
                Id = cartId,
                CustomerId = customerId,
                CustomerType = customerType,
                ShippingMethod = shippingMethod,
                ShippingAddress = address,
                Items = items,
            };
            return cart;
        }
    }
}
