using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Infrasturcture;
using SQLite;

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
        
        
        public async Task<List<Product>> GetProductsByCategory(Guid categoryId)
        {
            var db = await GetDb();
            
            return await db.Table<Product>().Where(p => p.CategoryId == categoryId).ToListAsync();
        }
    }
}