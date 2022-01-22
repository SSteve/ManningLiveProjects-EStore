using System;
namespace ShoppingCartService.DataAccess.Entities
{
    public class Coupon
    {
        public string Id { get; set; }
        public CouponType Type { get; set; }
        public double Value { get; set; }
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Coupons are considered equal if their Id properties are the same.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected bool Equals(Coupon other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Coupon)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
