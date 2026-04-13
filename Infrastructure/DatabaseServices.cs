using ProjectMaui.Domain.Models;
using SQLite;

namespace ProjectMaui.Domain.Infrasturcture;

public class DatabaseService
{
    private SQLiteAsyncConnection? database;
    private readonly string dbPath;

    public DatabaseService()
    {
        dbPath = Path.Combine(FileSystem.AppDataDirectory, "DbResto.sqlite");
    }

    public async Task<SQLiteAsyncConnection> GetConnection()
    {
        if (database is not null) return database;
        database = new SQLiteAsyncConnection(dbPath);

        await database.CreateTableAsync<Product>();
        await database.CreateTableAsync<Order>();
        await database.CreateTableAsync<OrderItem>();
        await database.CreateTableAsync<Inventory>();
        await database.CreateTableAsync<User>();
        await database.CreateTableAsync<Table>();

        return database;
    }
}