using System.Collections.Generic;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using Xunit;

// "sut" stands for "system under test"

namespace ShippingCartTests
{
    public class ShippingCalculatorTests
    {
        [Fact]
        public void Shipping_calculator_is_created_successfully()
        {
            var sut = new ShippingCalculator();

            Assert.NotNull(sut);
        }


        [Fact]
        public void Shipping_to_same_country_is_calculated_correctly()
        {
            uint numberOfItems = 8;
            var customerCity = "Grass Valley";
            var warehouseCity = "Chicago";
            var country = "USA";
            var cart = CreateCart(numberOfItems, city: customerCity, country: country,
                ShippingMethod.Standard);
            var sut = ShippingCalculatorWithCityAndCountry(city: warehouseCity, country: country);

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(cost, numberOfItems * ShippingCalculator.SameCountryRate);
        }

        [Fact]
        public void Shipping_to_same_city_is_calculated_correctly()
        {
            uint numberOfItems = 8;
            var customerCity = "Grass Valley";
            var warehouseCity = "Grass Valley";
            var country = "USA";
            var cart = CreateCart(numberOfItems, city: customerCity, country: country,
                ShippingMethod.Standard);
            var sut = ShippingCalculatorWithCityAndCountry(city: warehouseCity, country: country);

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(cost, numberOfItems * ShippingCalculator.SameCityRate);
        }

        [Fact]
        public void Shipping_to_other_country_is_calculated_correctly()
        {
            uint numberOfItems = 8;
            var warehouseCity = "Grass Valley";
            var warehouseCountry = "USA";
            var customerCity = "Oslo";
            var customerCountry = "Norway";
            var cart = CreateCart(numberOfItems, city: customerCity, country: customerCountry,
                shippingMethod: ShippingMethod.Standard);
            var sut = ShippingCalculatorWithCityAndCountry(city: warehouseCity, country: warehouseCountry);

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(cost, numberOfItems * ShippingCalculator.InternationalShippingRate);
        }

        [InlineData(ShippingMethod.Expedited)]
        [InlineData(ShippingMethod.Priority)]
        [InlineData(ShippingMethod.Express)]
        [Theory]
        public void Faster_shipping_is_calculated_correctly(ShippingMethod shippingMethod)
        {
            // These values are private in ShippingCalculator. They should be public to
            // facilitate testing.
            var shippingIncreaseBasedOnShippingMethod =
            new Dictionary<ShippingMethod, double>()
            {
                { ShippingMethod.Standard, 1.0 },
                { ShippingMethod.Expedited, 1.2 },
                { ShippingMethod.Priority, 2.0 },
                { ShippingMethod.Express, 2.5 },
            };
            uint numberOfItems = 8;
            var warehouseCity = "Grass Valley";
            var warehouseCountry = "USA";
            var customerCity = "Grass Valley";
            var customerCountry = "USA";
            var cart = CreateCart(numberOfItems, city: customerCity, country: customerCountry,
                shippingMethod);
            var sut = ShippingCalculatorWithCityAndCountry(city: warehouseCity, country: warehouseCountry);

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(cost, shippingIncreaseBasedOnShippingMethod[shippingMethod] * numberOfItems * ShippingCalculator.SameCityRate);

        }

        [InlineData(ShippingMethod.Expedited)]
        [InlineData(ShippingMethod.Priority)]
        [Theory]
        public void Faster_shipping_to_premium_customer_is_calculated_correctly(ShippingMethod shippingMethod)
        {
            // There is no multiplier for shipping methods Expedited or Priority for Premium customers.
            uint numberOfItems = 8;
            var warehouseCity = "Grass Valley";
            var warehouseCountry = "USA";
            var customerCity = "Grass Valley";
            var customerCountry = "USA";
            var cart = CreateCart(numberOfItems, city: customerCity, country: customerCountry,
                shippingMethod, CustomerType.Premium);
            var sut = ShippingCalculatorWithCityAndCountry(city: warehouseCity, country: warehouseCountry);

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(cost, numberOfItems * ShippingCalculator.SameCityRate);
        }

        [InlineData(ShippingMethod.Express)]
        [Theory]
        public void Express_shipping_to_premium_customer_is_calculated_correctly(ShippingMethod shippingMethod)
        {
            // Premium customers still pay the shipping multiplier for Express shipping.
            // These values are private in ShippingCalculator. They should be public to
            // facilitate testing.
            var shippingIncreaseBasedOnShippingMethod =
            new Dictionary<ShippingMethod, double>()
            {
                { ShippingMethod.Standard, 1.0 },
                { ShippingMethod.Expedited, 1.2 },
                { ShippingMethod.Priority, 2.0 },
                { ShippingMethod.Express, 2.5 },
            };
            uint numberOfItems = 8;
            var warehouseCity = "Grass Valley";
            var warehouseCountry = "USA";
            var customerCity = "Grass Valley";
            var customerCountry = "USA";
            var cart = CreateCart(numberOfItems, city: customerCity, country: customerCountry,
                shippingMethod, CustomerType.Premium);
            var sut = ShippingCalculatorWithCityAndCountry(city: warehouseCity, country: warehouseCountry);

            var cost = sut.CalculateShippingCost(cart);

            // There is no multiplier for shipping methods Expedited or Priority for Premium customers.
            Assert.Equal(cost, shippingIncreaseBasedOnShippingMethod[ShippingMethod.Express]
                 * numberOfItems * ShippingCalculator.SameCityRate);
        }

        private ShippingCalculator ShippingCalculatorWithCityAndCountry(string city, string country)
        {
            var address = new Address
            {
                Street = "Doesn't matter for calculating shipping.",
                City = city,
                Country = country
            };
            var calculator = new ShippingCalculator(address);
            return calculator;
        }

        private Cart CreateCart(uint numberOfItems, string city, string country,
            ShippingMethod shippingMethod, CustomerType customerType = CustomerType.Standard)
        {
            var cart = new Cart
            {
                ShippingAddress = new Address
                {
                    Street = "Doesn't matter for calculating shipping.",
                    City = city,
                    Country = country,
                },
                ShippingMethod = shippingMethod,
                CustomerType = customerType,
            };

            var item = new Item
            {
                ProductId = "id",
                ProductName = "Test Item 1",
                Price = 3.4,
                Quantity = numberOfItems
            };
            cart.Items.Add(item);
            return cart;
        }
    }
}
