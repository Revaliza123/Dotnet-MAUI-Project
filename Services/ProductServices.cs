using ProjectMaui.Models;
using SQLite;

namespace DotnetMauiProject.Services
{
    public class ProductServices
    {
        private SQLiteAsyncConnection _database;

        private async Task Init()
        {
            if (_database is not null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "Restaurant.db3");

            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<Product>();
        }
    }
}