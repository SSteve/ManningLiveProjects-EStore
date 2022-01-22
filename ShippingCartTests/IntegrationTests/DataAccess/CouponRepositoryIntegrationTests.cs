using System;
using MongoDB.Driver;
using ShippingCartTests.IntegrationTests.Fixtures;
using ShippingCartTests.Builders;
using ShoppingCartService.Config;
using ShoppingCartService.DataAccess;
using Xunit;

namespace ShippingCartTests.IntegrationTests.DataAccess
{
    [Collection("Dockerized MongoDB collection")]
    public class CouponRepositoryIntegrationTests : IDisposable
    {
        private readonly CouponDatabaseSettings _databaseSettings;

        public CouponRepositoryIntegrationTests(DockerMongoFixture fixture)
        {
            _databaseSettings = fixture.GetCouponDatabaseSettings();        
        }

        [Fact]
        public void Database_created_successfully()
        {
            var sut = new CouponRepository(_databaseSettings);

            Assert.NotNull(sut);
        }

        [Fact]
        public void Empty_database_returns_no_coupons()
        {
            var sut = new CouponRepository(_databaseSettings);

            var coupons = sut.FindAll();

            Assert.Empty(coupons);
        }

        [Fact]
        public void Database_with_one_coupon_returns_one_coupon()
        {
            var sut = new CouponRepository(_databaseSettings);
            sut.Create(CouponBuilder.BuildCoupon());

            var coupons = sut.FindAll();

            Assert.Single(coupons);
        }

        [Fact] void Can_create_coupon()
        {
            const string couponId = "322JF";

            var sut = new CouponRepository(_databaseSettings);
            var coupon = sut.Create(CouponBuilder.BuildCoupon(id: couponId));
            var allCoupons = sut.FindAll();

            Assert.NotNull(coupon);
            Assert.Single(allCoupons);
        }

        [Fact]
        public void Can_find_coupon_by_id()
        {
            const string couponId = "LWJ-03390";

            var sut = new CouponRepository(_databaseSettings);
            sut.Create(CouponBuilder.BuildCoupon(id: couponId));

            var coupon = sut.FindById(couponId);

            Assert.NotNull(coupon);
        }

        [Fact]
        public void Can_remove_coupon_by_reference()
        {
            const string couponId = "###111###";

            var sut = new CouponRepository(_databaseSettings);
            sut.Create(CouponBuilder.BuildCoupon(id: couponId));
            var coupon = sut.FindById(couponId);
            sut.Remove(coupon);

            var coupons = sut.FindAll();

            Assert.Empty(coupons);
        }

        [Fact]
        public void Can_remove_coupon_by_id()
        {
            const string couponId = "jJoO0";

            var sut = new CouponRepository(_databaseSettings);
            sut.Create(CouponBuilder.BuildCoupon(id: couponId));
            sut.Remove(couponId);

            var coupons = sut.FindAll();

            Assert.Empty(coupons);
        }

        public void Dispose()
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            client.DropDatabase(_databaseSettings.DatabaseName);
        }
    }
}
