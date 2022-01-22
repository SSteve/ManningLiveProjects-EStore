using System.Collections.Generic;
using ShoppingCartService.DataAccess.Entities;

namespace ShoppingCartService.DataAccess
{
    public interface ICouponRepository
    {
        IEnumerable<Coupon> FindAll();
        Coupon FindById(string id);
        Coupon Create(Coupon coupon);
        void Remove(Coupon coupon);
        void Remove(string id);
    }
}
