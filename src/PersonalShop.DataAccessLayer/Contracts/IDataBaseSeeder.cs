namespace PersonalShop.DataAccessLayer.Contracts;

public interface IDataBaseSeeder
{
    public Task<bool> MigrateAsync();
}
