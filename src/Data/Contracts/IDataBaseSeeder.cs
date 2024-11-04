namespace PersonalShop.Data.Contracts;

public interface IDataBaseSeeder
{
    public Task<bool> MigrateAsync();
}
