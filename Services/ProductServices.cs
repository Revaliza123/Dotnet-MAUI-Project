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
            try {
                var dbConect = await databaseServices.GetConnection();
                return await dbConect.Table<Product>().ToListAsync();
            }
            catch (Exception exc) {
                Console.WriteLine($"Error {exc.Message} when get the data");
                throw;
            }
        }

        public async Task AddProduct(Product product)
        {
            try {
                var db = await databaseServices.GetConnection();
                int result = await db.InsertAsync(product);

                if (result > 0) {
                    Console.WriteLine($"Succes to add the new data");
                }
            }
            catch (Exception exc) {
                Console.WriteLine($"Error {exc.Message} when get the data");
                throw;
            }
        }
    }
}