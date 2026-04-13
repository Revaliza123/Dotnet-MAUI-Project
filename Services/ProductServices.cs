using System.Diagnostics;
using ProjectMaui.Models;
using ProjectMaui.Services;
using SQLite;

namespace ProjectMaui.Services
{
    public class ProductServices
    {
        private readonly DatabaseService? databaseServices;
        private SQLiteAsyncConnection? connection;

        public ProductServices(DatabaseService database)
        {
            databaseServices = database;
        }

        private async Task<SQLiteAsyncConnection> GetDb()
        {
            if (connection == null) {
                connection = await databaseServices.GetConnection();
            }
            return connection;
        }
        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var dbConect = await GetDb();
                return await dbConect.Table<Product>().ToListAsync();  
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Error {exc.Message} when get the data");
                throw;
            }
        }

        public async Task AddProduct(Product product)
        {
            try
            {
                var db = await GetDb();
                int result = await db.InsertAsync(product);

                if (result > 0)
                {
                    Console.WriteLine($"Succes to add the new data");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Error {exc.Message} when get the data");
                throw;
            }
        }
        public async Task UpdateProduct(Product product)
        {
            try
            {
                var db = await GetDb();
                int result = await db.UpdateAsync(product);

                if (result > 0)
                {
                    Console.WriteLine($"Succes to update this data");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Error {exc.Message} when update the data");
                throw;
            }
        }

        public async Task DeleteProduct(Guid productId)
        {
            var db = await GetDb();
            await db.DeleteAsync<Product>(productId);
        }
    }
}