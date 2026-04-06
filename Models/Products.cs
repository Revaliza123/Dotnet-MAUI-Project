using SQLite;

namespace ProjectMaui.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public TimeSpan PreparationTime { get; set; }
        public ProductStatus Status { get; set; }

        public void ChangeAvailablelity(ProductStatus status, int ProductId) {
            Console.WriteLine($"{ProductId} Product");
            Console.WriteLine($"{status} Status Method");
        }
        public void UpdatePrice(decimal price, int ProductId) {
            Console.WriteLine($"{ProductId} products");
            Console.WriteLine($"{price} Status Method");
        }
        public void GetProductDetails(int ProductId) {
            Console.WriteLine($"{ProductId} products");
        }
    }

    public class Food : Product
    {
        public List<string> Ingredients { get; set; } = new();
        public string Taste { get; set; }
        public string NutritionInfo { get; set; }

        public void GetNutritionInfo(int ProductId) {
            Console.WriteLine($"{ProductId} products");
        }
    }

    public class Dessert : Food
    {
        public int SweetnessLevel { get; set; }
        public ServingTemp ServingTemp { get; set; }
        public void GetServingInstruction(int ProductId) {
            Console.WriteLine($"{ProductId} products");
        }
    }

    public class Drink : Product
    {
        public List<string> Ingredients { get; set; } = new();
        public SugarLevel SugarLevel { get; set; }
        public bool IsCaffeinated { get; set; }

        public void AdjustSugarLevel(int ProductId, SugarLevel sugarLevel) {
            Console.WriteLine($"{ProductId} products");
            Console.WriteLine($"{sugarLevel} sugar");
        }
    }
}