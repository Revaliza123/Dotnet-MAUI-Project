using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Infrasturcture;
using SQLite;

namespace ProjectMaui.Domain.Services
{
    public class InventoryServices
    {
        private readonly DatabaseService _databaseServices;
        private SQLiteAsyncConnection? _connection;

        public InventoryServices(DatabaseService database)
        {
            _databaseServices = database;
        }

        private async Task<SQLiteAsyncConnection> GetDb()
        {
            if (_connection == null) _connection = await _databaseServices.GetConnection();
            return _connection;
        }

        
        public async Task AddInventory(Inventory inv) => await (await GetDb()).InsertAsync(inv);
        public async Task UpdateInventory(Inventory inv) => await (await GetDb()).UpdateAsync(inv);
        public async Task DeleteInventory(Inventory inv) => await (await GetDb()).DeleteAsync(inv);

        
        public async Task<bool> CheckAvailability(Guid productId)
        {
            var db = await GetDb();
            var item = await db.Table<Inventory>().FirstOrDefaultAsync(x => x.ProductId == productId);
            return item != null && item.CurrentStock > 0;
        }

        public async Task<bool> IsLowStock(Guid productId)
        {
            var db = await GetDb();
            var item = await db.Table<Inventory>().FirstOrDefaultAsync(x => x.ProductId == productId);
            return item != null && item.CurrentStock <= item.MinimumStock;
        }
    }
}