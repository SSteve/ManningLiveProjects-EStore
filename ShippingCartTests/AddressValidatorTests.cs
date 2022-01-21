using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;
using Xunit;

// "sut" stands for "system under test"

namespace ShippingCartTests
{
    public class AddressValidatorTests
    {
        [InlineData("", "1267 Morningside Ave.", "USA")]
        [InlineData("South San Francisco", "", "USA")]
        [InlineData("South San Francisco", "1267 Morningside Ave.", "")]
        [Theory]
        public void Address_with_missing_data_is_invalid(
            string city,
            string street,
            string country)
        {
            var address = new Address
            {
                City = city,
                Street = street,
                Country = country,
            };
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void Address_with_all_fields_is_valid()
        {
            var address = new Address
            {
                City = "South San Francisco",
                Street = "1267 Morningside Ave.",
                Country = "USA"
            };
            var sut = new AddressValidator();

            var isValid = sut.IsValid(address);

            Assert.True(isValid);
        }
    }
}
