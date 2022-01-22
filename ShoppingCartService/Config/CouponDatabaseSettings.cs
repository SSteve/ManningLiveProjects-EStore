namespace ShoppingCartService.Config
{
    public class CouponDatabaseSettings : ICouponDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
