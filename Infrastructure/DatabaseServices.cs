using ProjectMaui.Domain.Models;
using ProjectMaui.Infrastructure.Entities;
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
        database = new SQLiteAsyncConnection(dbPath,
    SQLiteOpenFlags.ReadWrite |
    SQLiteOpenFlags.Create |
    SQLiteOpenFlags.SharedCache);

        await database.CreateTableAsync<Food>();
        await database.CreateTableAsync<Drink>();
        await database.CreateTableAsync<Dessert>();

        await database.CreateTableAsync<Employee>();
        await database.CreateTableAsync<Customer>();

        await database.CreateTableAsync<Order>();
        await database.CreateTableAsync<OrderItem>();

        await database.CreateTableAsync<Category>();
        await database.CreateTableAsync<Inventory>();
        await database.CreateTableAsync<Table>();

        return database;
    }
}