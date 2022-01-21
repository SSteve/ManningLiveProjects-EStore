using System;
using System.Linq;
using MongoDB.Driver;
using ShippingCartTests.IntegrationTests.Fixtures;
using ShoppingCartService.Config;
using ShoppingCartService.DataAccess;
using ShoppingCartService.DataAccess.Entities;
using Xunit;

namespace ShippingCartTests.IntegrationTests.DataAccess
{
    [Collection("Dockerized MongoDB collection")]
    public class ShoppingCartRepositoryIntegrationTests : IDisposable
    {
        private readonly ShoppingCartDatabaseSettings _databaseSettings;

        public ShoppingCartRepositoryIntegrationTests(DockerMongoFixture fixture)
        {
            _databaseSettings = fixture.GetDatabaseSettings();
        }

        [Fact]
        public void Database_created_successfully()
        {
            var sut = new ShoppingCartRepository(_databaseSettings);

            Assert.NotNull(sut);
        }

        [Fact]
        public void Empty_database_returns_no_carts()
        {
            var sut = new ShoppingCartRepository(_databaseSettings);

            var carts = sut.FindAll();

            Assert.Empty(carts);
        }

        [Fact]
        public void Database_with_one_cart_returns_one_cart()
        {
            var sut = new ShoppingCartRepository(_databaseSettings);
            sut.Create(BuildCart());

            var carts = sut.FindAll();

            Assert.Single(carts);
        }

        [Fact]
        public void Can_find_cart_by_id()
        {
            const string cartId = "34AA99956EABC0DF9E66AD15";
            var sut = new ShoppingCartRepository(_databaseSettings);
            sut.Create(BuildCart(id: cartId));

            var cart = sut.FindById(cartId);

            Assert.NotNull(cart);
        }

        [Fact]
        public void Can_create_cart()
        {
            const string cartId = "398324AA999560DFFDEA642C";

            var sut = new ShoppingCartRepository(_databaseSettings);
            var cart = sut.Create(BuildCart(id: cartId));
            var allCarts = sut.FindAll();

            Assert.NotNull(cart);
            Assert.Single(allCarts);
        }

        [Fact]
        public void Can_update_cart()
        {
            const string cartId = "C9F324AA12956ADFFDEA642C";
            const string customerId = "New customer id";

            var sut = new ShoppingCartRepository(_databaseSettings);
            var cart = sut.Create(BuildCart(id: cartId));
            cart.CustomerId = customerId;
            sut.Update(cartId, cart);
            var newCart = sut.FindById(cartId);            

            Assert.Equal(customerId, customerId);
        }

        [Fact]
        public void Can_remove_cart_by_reference()
        {
            const string cartId = "C9F324AA19ab47ef1DEA642C";

            var sut = new ShoppingCartRepository(_databaseSettings);
            sut.Create(BuildCart(id: cartId));
            var cart = sut.FindById(cartId);
            sut.Remove(cart);

            var carts = sut.FindAll();

            Assert.Empty(carts);
        }

        [Fact]
        public void Can_remove_cart_by_id()
        {
            const string cartId = "C9F324AA19ab47ef1DEA642C";

            var sut = new ShoppingCartRepository(_databaseSettings);
            sut.Create(BuildCart(id: cartId));
            sut.Remove(cartId);

            var carts = sut.FindAll();

            Assert.Empty(carts);
        }

        [Fact]
        public void Correct_cart_is_removed_when_there_are_multiple_carts()
        {
            const string cartId1 = "11F324AA12956ADFFDEA642C";
            const string cartId2 = "22F324AA12956ADFFDEA642C";
            const string cartId3 = "33F324AA12956ADFFDEA642C";

            var sut = new ShoppingCartRepository(_databaseSettings);
            sut.Create(BuildCart(id: cartId1));
            sut.Create(BuildCart(id: cartId2));
            sut.Create(BuildCart(id: cartId3));
            sut.Remove(cartId2);

            var cart = sut.FindById(cartId2);
            var carts = sut.FindAll();

            Assert.Null(cart);
            Assert.Equal(2, carts.Count());
        }

        public void Dispose()
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            client.DropDatabase(_databaseSettings.DatabaseName);
        }

        private Cart BuildCart(string id = "", string customerId = "")
        {
            var cart = new Cart
            {
                Id = id,
                CustomerId = customerId,
            };
            return cart;
        }
    }
}
