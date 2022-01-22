using System;
using System.Collections.Generic;
using MongoDB.Driver;
using ShoppingCartService.Config;
using ShoppingCartService.DataAccess.Entities;

namespace ShoppingCartService.DataAccess
{
    public class CouponRepository : ICouponRepository
    {
        private readonly IMongoCollection<Coupon> _coupons;
        
        public CouponRepository(ICouponDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _coupons = database.GetCollection<Coupon>(settings.CollectionName);
        }

        public Coupon Create(Coupon coupon)
        {
            _coupons.InsertOne(coupon);

            return (coupon);
        }

        public IEnumerable<Coupon> FindAll() => _coupons.Find(_ => true).ToEnumerable();

        public Coupon FindById(string id) =>
            _coupons.Find(coupon => coupon.Id == id)
                .FirstOrDefault();

        public void Remove(Coupon coupon) => _coupons.DeleteOne(c => c.Id == coupon.Id);
        public void Remove(string id) => _coupons.DeleteOne(c => c.Id == id);
    }
}
