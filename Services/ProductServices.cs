using System.Diagnostics;
using DatabaseServices;
using ProjectMaui.Models;
using SQLite;

namespace Services
{
    public class ProductServices
    {

        private readonly DatabaseServiceConnection? databaseServices;

        public ProductServices(DatabaseServiceConnection database)
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