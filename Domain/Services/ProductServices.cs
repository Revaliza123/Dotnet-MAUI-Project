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
        private readonly DatabaseService? databaseServices;
        private SQLiteAsyncConnection? connection;

        public ProductServices(DatabaseService database)
        {
            databaseServices = database;
        }

        private async Task<SQLiteAsyncConnection> GetDb()
        {
            if (connection == null)
            {
                connection = await databaseServices.GetConnection();
            }
            return connection;
        }
        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var dbConect = await GetDb();
                var dataEntity = await dbConect.Table<ProductEntity>().ToListAsync();
                var products = new List<Product>();

                foreach (var entity in dataEntity)
                {
                    Product product = entity.ProductType switch
                    {
                        nameof(Product.ProductTypes.Drink) => new Drink(
                            entity.Name,
                            entity.Description,
                            entity.Price,
                            entity.Image,
                            entity.Ingredients,
                            entity.StockQuantity,
                            entity.PreparationTime,
                            (ProductStatus)entity.Status,
                            (SugarLevel)entity.SugarLevel,
                            entity.IsCaffeinated)
                        { Id = entity.Id },

                        nameof(Product.ProductTypes.Dessert) => new Dessert(
                            entity.Name,
                            entity.Description,
                            entity.Price,
                            entity.Image,
                            entity.Ingredients,
                            entity.StockQuantity,
                            entity.PreparationTime,
                            (ProductStatus)entity.Status,
                            entity.Taste, entity.NutritionInfo,
                            entity.SweetnessLevel,
                            (ServingTemp)entity.ServingTemp)
                        { Id = entity.Id },

                        nameof(Product.ProductTypes.Food) => new Food(
                            entity.Name,
                            entity.Description,
                            entity.Price,
                            entity.Image,
                            entity.Ingredients,
                            entity.StockQuantity,
                            entity.PreparationTime,
                            (ProductStatus)entity.Status,
                            entity.Taste,
                            entity.NutritionInfo)
                        { Id = entity.Id },

                        _ => new Product(
                            entity.Name,
                            entity.Description,
                            entity.Price,
                            entity.Image,
                            entity.Ingredients,
                            entity.StockQuantity,
                            entity.PreparationTime,
                            (ProductStatus)entity.Status)
                        { Id = entity.Id }
                    };

                    products.Add(product);
                }

                return products;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Error {exc.Message} when get the data");
                throw;
            }
        }

        public async Task<List<ProductWithCategory>> GetProductWithCategoryAsync()
        {
            try
            {
                var db = await GetDb();
                var products = await GetProductsAsync();
                var category = await db.Table<Category>().ToListAsync();
                var result = new List<ProductWithCategory>();

                foreach (var prodCat in products)
                {
                    var matchedCategory = category.FirstOrDefault(c => c.Id == prodCat.CategoryId);

                    result.Add(new ProductWithCategory
                    {
                        Product = prodCat,
                        CategoryName = matchedCategory != null ? matchedCategory.Name : "Not any data matched"
                    });
                }

                return result;
            }
            catch (System.Exception exc)
            {
                Console.WriteLine($"Error {exc.Message} when get data with category");
                throw;
            }
        }

        public async Task AddProduct(Product product)
        {
            try
            {
                var db = await GetDb();
                var mapedEntity = MapToEntity(product);
                int result = await db.InsertAsync(mapedEntity);

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
                var mappedEntity = MapToEntity(product);
                int result = await db.UpdateAsync(mappedEntity);

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
            await db.Table<ProductEntity>().DeleteAsync(p => p.Id == productId);
        }

        private ProductEntity MapToEntity(Product product)
        {
            var entity = new ProductEntity
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Image = product.Image,
                Ingredients = product.Ingredients,
                StockQuantity = product.StockQuantity,
                PreparationTime = product.PreparationTime,
                Status = (int)product.Status
            };

            if (product is Drink drink)
            {
                entity.ProductType = nameof(Product.ProductTypes.Drink);
                entity.SugarLevel = (int)drink.SugarLevel;
                entity.IsCaffeinated = drink.IsCaffeinated;
            }
            else if (product is Dessert dessert)
            {
                entity.ProductType = nameof(Product.ProductTypes.Dessert);
                entity.Taste = dessert.Taste;
                entity.NutritionInfo = dessert.NutritionInfo;
                entity.SweetnessLevel = dessert.SweetnessLevel;
                entity.ServingTemp = (int)dessert.ServingTemp;
            }
            else if (product is Food food)
            {
                entity.ProductType = nameof(Product.ProductTypes.Food);
                entity.Taste = food.Taste;
                entity.NutritionInfo = food.NutritionInfo;
            }
            else
            {
                entity.ProductType = "General";
            }

            return entity;
        }
    }
}