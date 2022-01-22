using System;
using System.Runtime.Serialization;

namespace ShoppingCartService.BusinessLogic.Exceptions
{
    [Serializable]
    public class CouponExpiredException : Exception
    {
        public CouponExpiredException()
        {
        }

        public CouponExpiredException(string message) : base(message)
        {
        }

        public CouponExpiredException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CouponExpiredException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}