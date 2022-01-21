using System;
using System.Collections.Generic;
using AutoMapper;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;
using Xunit;

namespace ShippingCartTests.UnitTests
{
    public class CheckoutEngineTests
    {
        // Create IMapper as a member of the test class
        private readonly IMapper _mapper;

        // These values are copied from ShippingCalculator.cs. They should have a public accessor
        // in that class to facilitate testing.
        private readonly Dictionary<ShippingMethod, double> _shippingIncreaseBasedOnShippingMethod =
            new()
            {
                { ShippingMethod.Standard, 1.0 },
                { ShippingMethod.Expedited, 1.2 },
                { ShippingMethod.Priority, 2.0 },
                { ShippingMethod.Express, 2.5 },
            };

        // Use the class constructor to initialize IMapper
        public CheckoutEngineTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Premium_customer_has_discount()
        {
            var sut = BuildCheckOutEngine();
            var cart = BuildCart(customerType: CustomerType.Premium);

            var checkoutDto = sut.CalculateTotals(cart);

            Assert.Equal(10.0, checkoutDto.CustomerDiscount);
        }

        [Fact]
        public void Standard_customer_has_no_discount()
        {
            var sut = BuildCheckOutEngine();
            var cart = BuildCart(customerType: CustomerType.Standard);

            var checkoutDto = sut.CalculateTotals(cart);

            Assert.Equal(0.0, checkoutDto.CustomerDiscount);
        }

        [InlineData(15, 17.5, 4, 31.87, ShippingMethod.Standard)]
        [InlineData(3, 21.00, 8, 32.61, ShippingMethod.Expedited)]
        [InlineData(7, 33.11, 12, 12.12, ShippingMethod.Priority)]
        [InlineData(63, 1.25, 17, 19.99, ShippingMethod.Express)]
        [Theory]
        public void Premium_customer_total_is_correct(
            uint item1Quantity, float item1Price,
            uint item2Quantity, float item2Price,
            ShippingMethod shippingMethod)
        {
            var items = BuildItems(new List<Tuple<uint, float>>
            {
                new (item1Quantity, item1Price),
                new (item2Quantity, item2Price),
            });
            var cart = BuildCart(customerType: CustomerType.Premium,
                shippingMethod: shippingMethod,
                items: items);
            var shippingCost = new ShippingCalculator().CalculateShippingCost(cart);
            var sut = BuildCheckOutEngine();

            var checkoutDto = sut.CalculateTotals(cart);

            // Note: Currency should never be represented with floating-point values. Having
            // to use Math.Round() to get the expected value to equal the calculated value
            // is a sign of a flawed model.
            var expected = Math.Round((shippingCost +
                item1Quantity * item1Price +
                item2Quantity * item2Price) * 0.9, 2);

            Assert.Equal(expected, Math.Round(checkoutDto.Total, 2));
        }

        [InlineData(15, 17.5, 4, 31.87, ShippingMethod.Standard)]
        [InlineData(3, 21.00, 8, 32.61, ShippingMethod.Expedited)]
        [InlineData(7, 33.11, 12, 12.12, ShippingMethod.Priority)]
        [InlineData(63, 1.25, 17, 19.99, ShippingMethod.Express)]
        [Theory]
        public void Standard_customer_total_is_correct(
            uint item1Quantity, float item1Price,
            uint item2Quantity, float item2Price,
            ShippingMethod shippingMethod)
        {
            var items = BuildItems(new List<Tuple<uint, float>>
            {
                new (item1Quantity, item1Price),
                new (item2Quantity, item2Price),
            });
            var cart = BuildCart(customerType: CustomerType.Standard,
                shippingMethod: shippingMethod,
                items: items);
            var shippingCost = new ShippingCalculator().CalculateShippingCost(cart);
            var sut = BuildCheckOutEngine();

            var checkoutDto = sut.CalculateTotals(cart);
            var expected = Math.Round(shippingCost +
                item1Quantity * item1Price +
                item2Quantity * item2Price, 2);

            Assert.Equal(expected, Math.Round(checkoutDto.Total, 2));
        }


        private CheckOutEngine BuildCheckOutEngine()
        {
            var checkOutEngine = new CheckOutEngine(new ShippingCalculator(), _mapper);
            return checkOutEngine;
        }

        private List<Item> BuildItems(List<Tuple<uint, float>> itemValues)
        {
            var items = new List<Item>();
            foreach (var itemValue in itemValues)
            {
                items.Add(new()
                {
                    Quantity = itemValue.Item1,
                    Price = itemValue.Item2,
                    ProductId = "id",
                    ProductName = "name"
                });
            }
            return items;
        }

        /// <summary>
        /// Build a cart with test values.
        /// </summary>
        /// <returns></returns>
        private Cart BuildCart(CustomerType customerType = CustomerType.Standard,
            ShippingMethod shippingMethod = ShippingMethod.Standard,
            Address address = null,
            List<Item> items = null)
        {
            var cartId = "";
            var customerId = "";
            if (address is null)
            {
                address = new Address
                {
                    Street = "Street",
                    City = "Dallas",
                    Country = "USA",
                };
            }
            if (items is null)
            {
                items = new List<Item>
                {
                    new()
                    {
                        Quantity = 2,
                        Price = 10.00,
                        ProductId = "",
                        ProductName = "",
                    }
                };
            }
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
