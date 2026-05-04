using Microsoft.Maui.Controls;
using ProjectMaui.Domain.Infrasturcture;
using ProjectMaui.Domain.Models;
using SQLite;

namespace ProjectMaui.Domain.Services;

public class InventoryServices : IInventoryManager
{
    private readonly DatabaseService _databaseService;
    private SQLiteAsyncConnection? _connection;

    public InventoryServices(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    private async Task<SQLiteAsyncConnection> GetDb()
    {
        if (_connection == null)
            _connection = await _databaseService.GetConnection();
        return _connection;
    }

    public async Task AddInventoryAsync(Inventory inventory)
    {
        var db = await GetDb();
        await db.InsertAsync(inventory);
        Console.WriteLine("Inventory record added");
    }

    public async Task UpdateInventoryAsync(Inventory inventory)
    {
        var db = await GetDb();
        await db.UpdateAsync(inventory);
        Console.WriteLine("Inventory record updated");
    }

    public async Task DeleteInventoryAsync(Guid inventoryId)
    {
        var db = await GetDb();
        var inventory = await db.Table<Inventory>().FirstOrDefaultAsync(x => x.Id == inventoryId);
        if (inventory != null)
        {
            await db.DeleteAsync(inventory);
            Console.WriteLine("Inventory record deleted");
        }
    }

    public async Task<bool> CheckAvailabilityAsync(Guid productId)
    {
        var db = await GetDb();
        var inventory = await db.Table<Inventory>().FirstOrDefaultAsync(x => x.ProductId == productId);
        return inventory != null && inventory.CurrentStock > 0;
    }

    public async Task<bool> IsLowStockAsync(Guid productId)
    {
        var db = await GetDb();
        var inventory = await db.Table<Inventory>().FirstOrDefaultAsync(x => x.ProductId == productId);
        if (inventory == null) return false;
        return inventory.CurrentStock <= inventory.MinimumStock;
    }

    public async Task<List<Inventory>> GetAllInventoryAsync()
    {
        var db = await GetDb();
        return await db.Table<Inventory>().ToListAsync();
    }

    public async Task<Inventory?> GetInventoryByProductIdAsync(Guid productId)
    {
        var db = await GetDb();
        return await db.Table<Inventory>().FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public async Task<List<Inventory>> GetLowStockItemsAsync()
    {
        var all = await GetAllInventoryAsync();
        return all.Where(x => x.CurrentStock <= x.MinimumStock).ToList();
    }
}
