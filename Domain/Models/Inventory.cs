using ProjectMaui.Domain.Common;
using SQLite;

namespace ProjectMaui.Domain.Models
{
    public class Inventory
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProductId { get; private set; }
        public int CurrentStock { get; private set; }
        public int MinimumStock { get; private set; }
        public DateTime LastUpdated { get; private set; }

        public Inventory() { }

        public Inventory(Guid productId, int currentStock, int minimumStock)
        {
            ProductId = productId;
            CurrentStock = Guard.AtLeast(currentStock, 0, nameof(currentStock));
            MinimumStock = Guard.AtLeast(minimumStock, 0, nameof(minimumStock));
            LastUpdated = DateTime.Now;
        }

        // Method Logic untuk SQLite 
        public void AddStock(int amount)
        {
            CurrentStock += Guard.AtLeast(amount, 1, nameof(amount));
            LastUpdated = DateTime.Now;
            Console.WriteLine($"Stock added. New total: {CurrentStock}");
        }

        public void ReduceStock(int amount)
        {
            int result = CurrentStock - amount;
            CurrentStock = Guard.AtLeast(result, 0, "Stock cannot be negative");
            LastUpdated = DateTime.Now;
        }
    }
}