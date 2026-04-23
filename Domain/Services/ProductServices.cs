using System.Diagnostics;
using ProjectMaui.Domain.DTOs;
using ProjectMaui.Domain.Infrasturcture;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;
using ProjectMaui.Infrastructure.Entities;
using SQLite;

namespace ProjectMaui.Domain.Services
{
    public class ProductServices
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
            var entities = await db.Table<ProductEntity>().ToListAsync();

            return entities.Select(p =>
            {
                Product product = p.ProductType switch
                {
                    "Food" => new Food(p.Name, p.Description, p.Price, p.Image, p.Ingredients, p.StockQuantity, p.PreparationTime, (ProductStatus)p.Status, p.Taste, p.NutritionInfo),
                    "Drink" => new Drink(p.Name, p.Description, p.Price, p.Image, p.Ingredients, p.StockQuantity, p.PreparationTime, (ProductStatus)p.Status, (SugarLevel)p.SugarLevel, p.IsCaffeinated),
                    "Dessert" => new Dessert(p.Name, p.Description, p.Price, p.Image, p.Ingredients, p.StockQuantity, p.PreparationTime, (ProductStatus)p.Status, p.Taste, p.NutritionInfo, p.SweetnessLevel, (ServingTemp)p.ServingTemp),
                    _ => throw new Exception("Unknown type")
                };
                product.Id = p.Id;
                return product;
            }).ToList();
        }
        private async Task<string> SaveImageLocally(FileResult photo, Product product)
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
            int result = 0;

            if (photo != null)
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

                product.Image = fullPath;
            }

            switch (type)
            {
                case Product.ProductTypes.Food:
                    result = await db.InsertAsync((Food)product);
                    break;
                case Product.ProductTypes.Drink:
                    result = await db.InsertAsync((Drink)product);
                    break;
                case Product.ProductTypes.Dessert:
                    result = await db.InsertAsync((Dessert)product);
                    break;
            }

            if (result > 0) Console.WriteLine($"Berhasil menambah {type} dengan gambar unik.");
        }

        public async Task UpdateProduct(Product product, Product.ProductTypes type)
        {
            var db = await GetDb();
            int result = 0;

            switch (type)
            {
                case Product.ProductTypes.Food:
                    result = await db.UpdateAsync((Food)product);
                    break;
                case Product.ProductTypes.Drink:
                    result = await db.UpdateAsync((Drink)product);
                    break;
                case Product.ProductTypes.Dessert:
                    result = await db.UpdateAsync((Dessert)product);
                    break;
            }

            if (result > 0) Console.WriteLine($"Berhasil update {type}");
        }

        public async Task DeleteProduct(Guid productId, Product.ProductTypes type)
        {
            var db = await GetDb();

            switch (type)
            {
                case Product.ProductTypes.Food:
                    await db.DeleteAsync<Food>(productId);
                    break;
                case Product.ProductTypes.Drink:
                    await db.DeleteAsync<Drink>(productId);
                    break;
                case Product.ProductTypes.Dessert:
                    await db.DeleteAsync<Dessert>(productId);
                    break;
            }

            Console.WriteLine($"Berhasil menghapus {type}");
        }


    }
}