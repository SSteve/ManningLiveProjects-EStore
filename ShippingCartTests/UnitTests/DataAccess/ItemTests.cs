using ShoppingCartService.DataAccess.Entities;
using Xunit;

namespace ShippingCartTests.UnitTests
{
    /// <summary>
    /// The item class defines Equals and GetHashCode which need to be tested.
    /// </summary>
    public class ItemTests
    {
        [Fact]
        public void Same_items_are_equal()
        {
            var item1 = BuildItem();
            var item2 = BuildItem();

            var itemsAreEqual = Equals(item1, item2);

            Assert.True(itemsAreEqual);
        }

        [Fact]
        public void Different_items_are_not_equal()
        {
            var item1 = BuildItem();
            var item2 = BuildItem();
            item2.ProductId = "A different product id";

            var itemsAreEqual = Equals(item1, item2);

            Assert.False(itemsAreEqual);
        }

        [Fact]
        public void Same_items_have_same_hashcode()
        {
            var item1 = BuildItem();
            var item2 = BuildItem();

            var item1HashCode = item1.GetHashCode();
            var item2HashCode = item2.GetHashCode();

            Assert.Equal(item1HashCode, item2HashCode);
        }

        [Fact]
        public void Different_items_have_different_hashcodes()
        {
            var item1 = BuildItem();
            var item2 = BuildItem();
            item2.ProductId = "A different product id";

            var item1HashCode = item1.GetHashCode();
            var item2HashCode = item2.GetHashCode();

            Assert.NotEqual(item1HashCode, item2HashCode);
        }

        private Item BuildItem()
        {
            var item = new Item
            {
                ProductId = "prod777",
                ProductName = "Cool thing",
                Price = 32.36,
                Quantity = 932,
            };
            return item;
        }
    }
}
