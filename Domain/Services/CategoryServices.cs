using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Infrasturcture;
using SQLite;
using ProjectMaui.Infrastructure.Entities;

namespace ProjectMaui.Domain.Services
{
    public class CategoryServices
    {
        private readonly DatabaseService _databaseServices;
        private SQLiteAsyncConnection? _connection;

        public CategoryServices(DatabaseService database)
        {
            _databaseServices = database;
        }

        private async Task<SQLiteAsyncConnection> GetDb()
        {
            if (_connection == null) _connection = await _databaseServices.GetConnection();
            return _connection;
        }

        public async Task<List<Category>> GetAllCategories() => await (await GetDb()).Table<Category>().ToListAsync();

        public async Task AddCategory(Category cat) => await (await GetDb()).InsertAsync(cat);

        public async Task UpdateCategory(Category cat) => await (await GetDb()).UpdateAsync(cat);

        public async Task DeleteCategory(Guid id) => await (await GetDb()).DeleteAsync(id);


        public async Task<List<Product>> GetProductsByCategory(Guid categoryId)
        {
            var db = await GetDb();

            var entities = await db.Table<ProductEntity>().Where(p => p.CategoryId == categoryId).ToListAsync();

            var products = entities.Select(p =>
            {
                Product product = p.ProductType switch
                {
                    "Food" => new Food(
                        p.Name, p.Description, p.Price, p.Image, p.Ingredients,
                        p.StockQuantity, p.PreparationTime, (ProductStatus)p.Status,
                        p.Taste, p.NutritionInfo),

                    "Drink" => new Drink(
                        p.Name, p.Description, p.Price, p.Image, p.Ingredients,
                        p.StockQuantity, p.PreparationTime, (ProductStatus)p.Status,
                        (SugarLevel)p.SugarLevel, p.IsCaffeinated),

                    "Dessert" => new Dessert(
                        p.Name, p.Description, p.Price, p.Image, p.Ingredients,
                        p.StockQuantity, p.PreparationTime, (ProductStatus)p.Status,
                        p.Taste, p.NutritionInfo, p.SweetnessLevel, (ServingTemp)p.ServingTemp),

                    _ => throw new Exception("Tipe produk tidak dikenali")
                };

                product.Id = p.Id;
                return product;
            }).ToList();

            return products;
        }
    }
}