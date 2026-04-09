using System.Diagnostics;
using ProjectMaui.Models;
using ProjectMaui.Services;
using SQLite;

namespace ProjectMaui.Services
{
    public class ProductServices
    {

        private readonly DatabaseService? databaseServices;

        public ProductServices(DatabaseService database)
        {
            databaseServices = database;
        }
        public async Task<List<Product>> GetProductsAsync()
        {
            var dbConect = await databaseServices.GetConnection();
            return await dbConect.Table<Product>().ToListAsync();
        }
    }
}