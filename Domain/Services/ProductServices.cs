using System.Diagnostics;
using ProjectMaui.Domain.DTOs;
using ProjectMaui.Domain.Infrasturcture;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;
using ProjectMaui.Infrastructure.Entities;
using SQLite;

namespace ProjectMaui.Domain.Services
{
    public class ProductServices : IProductService
    {
        private readonly DatabaseService databaseServices;
        private SQLiteAsyncConnection? connection;

        public ProductServices(DatabaseService database)
        {
            databaseServices = database ?? throw new ArgumentNullException(nameof(database));
        }

        private async Task<SQLiteAsyncConnection> GetDb()
        {
            if (connection == null)
            {
                connection ??= await databaseServices.GetConnection();
            }
            return connection;
        }
        public async Task<List<Product>> GetAllProducts()
        {
            var db = await GetDb();
            var products = new List<Product>();

            var foods = await db.Table<Food>().ToListAsync();
            var drinks = await db.Table<Drink>().ToListAsync();
            var desserts = await db.Table<Dessert>().ToListAsync();

            products.AddRange(foods);
            products.AddRange(drinks);
            products.AddRange(desserts);

            return products;
        }
        public async Task<string> SaveImageLocally(FileResult photo, Product product)
        {
            var folderPath = Path.Combine(FileSystem.AppDataDirectory, "product-images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var extension = Path.GetExtension(photo.FileName);
            var fileName = $"{product.Id}_{product.Name.Replace(" ", "_")}{extension}";
            var fullPath = Path.Combine(folderPath, fileName);

            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(fullPath))
            {
                await stream.CopyToAsync(newStream);
            }

            return fullPath;
        }
        public async Task AddProduct(Product product, Product.ProductTypes type, FileResult? photo)
        {
            var db = await GetDb();

            if (photo != null)
            {


                var slug = product.Name.ToLower().Replace(" ", "_");
                var shortId = product.Id.ToString("N")[..8];
                var extension = Path.GetExtension(photo.FileName);
                var fileName = $"{slug}_{shortId}{extension}";
                var fullPath = Path.Combine(fileName);

                using var stream = await photo.OpenReadAsync();
                using var newStream = File.OpenWrite(fullPath);
                await stream.CopyToAsync(newStream);

                product.Image = $"{fileName}";
            }

            int result = type switch
            {
                Product.ProductTypes.Food => await db.InsertAsync((Food)product),
                Product.ProductTypes.Drink => await db.InsertAsync((Drink)product),
                Product.ProductTypes.Dessert => await db.InsertAsync((Dessert)product),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };

            if (result > 0)
                Console.WriteLine($"Berhasil menambah {type}: {product.Name}");
        }

        public async Task UpdateProduct(Product product, Product.ProductTypes type, FileResult? newPhoto = null)
        {
            var db = await GetDb();

            if (newPhoto != null)
            {
                var folderPath = Path.Combine(FileSystem.AppDataDirectory, "product-images");
                Directory.CreateDirectory(folderPath);

                var extension = Path.GetExtension(newPhoto.FileName);
                var fileName = $"{product.Id}_{product.Name.Replace(" ", "_")}{extension}";
                var fullPath = Path.Combine(folderPath, fileName);

                using var stream = await newPhoto.OpenReadAsync();
                using var newStream = File.OpenWrite(fullPath);
                await stream.CopyToAsync(newStream);

                product.Image = fullPath;
            }

            int result = type switch
            {
                Product.ProductTypes.Food => await db.UpdateAsync((Food)product),
                Product.ProductTypes.Drink => await db.UpdateAsync((Drink)product),
                Product.ProductTypes.Dessert => await db.UpdateAsync((Dessert)product),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };

            if (result > 0)
                Console.WriteLine($"Berhasil update {type}: {product.Name}");
        }
        public async Task DeleteProduct(Guid productId, Product.ProductTypes type)
        {
            var db = await GetDb();

            switch (type)
            {
                case Product.ProductTypes.Food:
                    var food = await db.Table<Food>().FirstOrDefaultAsync(f => f.Id == productId);
                    if (food != null) await db.DeleteAsync(food);
                    break;

                case Product.ProductTypes.Drink:
                    var drink = await db.Table<Drink>().FirstOrDefaultAsync(d => d.Id == productId);
                    if (drink != null) await db.DeleteAsync(drink);
                    break;

                case Product.ProductTypes.Dessert:
                    var dessert = await db.Table<Dessert>().FirstOrDefaultAsync(d => d.Id == productId);
                    if (dessert != null) await db.DeleteAsync(dessert);
                    break;
            }

            Console.WriteLine($"Berhasil menghapus {type} id: {productId}");
        }


    }
}