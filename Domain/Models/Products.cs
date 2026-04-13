using ProjectMaui.Domain.Common;
using SQLite;

namespace ProjectMaui.Domain.Models
{
    public class Product
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid? CategoryId { get; private set; }
        public decimal Price { get; private set; }
        public string Ingredients { get; private set; }
        public int StockQuantity { get; private set; }
        public TimeSpan PreparationTime { get; private set; }
        public ProductStatus Status { get; private set; }

        public Product() { }
        public Product(string name, string description, decimal price, string ingredients, int stockQuantity, TimeSpan preparationTime, ProductStatus status)
        {
            Name = Guard.NotNullOrWhiteSpace(name, nameof(name));
            Description = Guard.NotNullOrWhiteSpace(description, nameof(description));
            Price = Guard.NotNegative(price, nameof(price));
            Ingredients = ingredients;
            StockQuantity = Guard.AtLeast(stockQuantity, 1, nameof(stockQuantity));
            PreparationTime = preparationTime;
            Status = status;
        }

        public void ChangeAvailablelity(ProductStatus status, Guid productId)
        {
            Console.WriteLine($"Product ID: {productId} changed to status: {status}");
        }

        public void UpdatePrice(decimal price, Guid productId)
        {
            Price = Guard.NotNegative(price, nameof(price));
            Console.WriteLine($"Product ID: {productId} price updated to: {price}");
        }

        public void GetProductDetails(Guid productId)
        {
            Console.WriteLine($"Fetching details for Product ID: {productId}");
        }
    }

    public class Food : Product
    {
        public string Taste { get; set; }
        public string NutritionInfo { get; set; }

        public Food() : base() { }

        public Food(string name, string description, decimal price, string ingredients, int stockQuantity, TimeSpan prepTime, ProductStatus status, string taste, string nutritionInfo)
            : base(name, description, price, ingredients, stockQuantity, prepTime, status)
        {
            Taste = Guard.NotNullOrWhiteSpace(taste, nameof(taste));
            NutritionInfo = nutritionInfo;
        }

        public void GetNutritionInfo(Guid productId)
        {
            Console.WriteLine($"Nutrition Info for {productId}: {NutritionInfo}");
        }
    }

    public class Dessert : Food
    {
        public int SweetnessLevel { get; set; }
        public ServingTemp ServingTemp { get; set; }

        public Dessert() : base() { }

        public Dessert(string name, string description, decimal price, string ingredients, int stockQuantity, TimeSpan prepTime, ProductStatus status, string taste, string nutritionInfo, int sweetness, ServingTemp temp)
            : base(name, description, price, ingredients, stockQuantity, prepTime, status, taste, nutritionInfo)
        {
            SweetnessLevel = sweetness;
            ServingTemp = temp;
        }

        public void GetServingInstruction(Guid productId)
        {
            Console.WriteLine($"Serve {productId} at {ServingTemp} temperature.");
        }
    }

    public class Drink : Product
    {
        public SugarLevel SugarLevel { get; set; }
        public bool IsCaffeinated { get; set; }

        public Drink() : base() { }

        public Drink(string name, string description, decimal price, string ingredients, int stockQuantity, TimeSpan prepTime, ProductStatus status, SugarLevel sugar, bool caffeinated)
            : base(name, description, price, ingredients, stockQuantity, prepTime, status)
        {
            SugarLevel = sugar;
            IsCaffeinated = caffeinated;
        }

        public void AdjustSugarLevel(Guid productId, SugarLevel sugarLevel)
        {
            SugarLevel = sugarLevel;
            Console.WriteLine($"Sugar level for {productId} adjusted to {sugarLevel}");
        }
    }
}