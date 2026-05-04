using ProjectMaui.Domain.Models;

namespace ProjectMaui.Domain.Services
{
    public interface IInventoryManager
    {
        Task<List<Inventory>> GetAllInventoryAsync();
        Task<Inventory?> GetInventoryByProductIdAsync(Guid productId);
        Task AddInventoryAsync(Inventory inventory);
        Task UpdateInventoryAsync(Inventory inventory);
        Task DeleteInventoryAsync(Guid inventoryId);
        Task<bool> CheckAvailabilityAsync(Guid productId);
        Task<bool> IsLowStockAsync(Guid productId);
        Task<List<Inventory>> GetLowStockItemsAsync();
    }
}
