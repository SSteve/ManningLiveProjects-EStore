namespace ShoppingCartService.Config
{
    public interface ICouponDatabaseSettings
    {
        string CollectionName { get; }
        string ConnectionString { get; }
        string DatabaseName { get; }
    }
}
