using ProjectMaui.Domain.Common;
using SQLite;

namespace ProjectMaui.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public Guid? CategoryId { get; protected set; }
        public decimal Price { get; protected set; }
        public string Image { get; protected set; }
        public string Ingredients { get; protected set; }
        public int StockQuantity { get; protected set; }
        public TimeSpan PreparationTime { get; protected set; }
        public ProductStatus Status { get; protected set; }
        public enum ProductTypes { Food, Dessert, Drink }

        public Product() { }
        public Product(string name, string description, decimal price, string image, string ingredients, int stockQuantity, TimeSpan preparationTime, ProductStatus status)
        {
            Name = Guard.NotNullOrWhiteSpace(name, nameof(name));
            Description = Guard.NotNullOrWhiteSpace(description, nameof(description));
            Price = Guard.NotNegative(price, nameof(price));
            Image = image;
            Ingredients = ingredients;
            StockQuantity = Guard.AtLeast(stockQuantity, 1, nameof(stockQuantity));
            PreparationTime = preparationTime;
            Status = status;
        }

        public void SetCategory(Guid categoryId)
        {
            this.CategoryId = categoryId;
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

    [Table("Product")]
    public class Food : Product
    {
        public string Taste { get; set; }
        public string NutritionInfo { get; set; }

        public Food() : base() { }

        public Food(string name, string description, decimal price, string image, string ingredients, int stockQuantity, TimeSpan prepTime, ProductStatus status, string taste, string nutritionInfo)
            : base(name, description, price, image, ingredients, stockQuantity, prepTime, status)
        {
            Taste = Guard.NotNullOrWhiteSpace(taste, nameof(taste));
            NutritionInfo = nutritionInfo;
        }

        public void GetNutritionInfo(Guid productId)
        {
            Console.WriteLine($"Nutrition Info for {productId}: {NutritionInfo}");
        }
    }

    [Table("Product")]
    public class Dessert : Food
    {
        public int SweetnessLevel { get; set; }
        public ServingTemp ServingTemp { get; set; }

        public Dessert() : base() { }

        public Dessert(string name, string description, decimal price, string image, string ingredients, int stockQuantity, TimeSpan prepTime, ProductStatus status, string taste, string nutritionInfo, int sweetness, ServingTemp temp)
            : base(name, description, price, image, ingredients, stockQuantity, prepTime, status, taste, nutritionInfo)
        {
            SweetnessLevel = sweetness;
            ServingTemp = temp;
        }

        public void GetServingInstruction(Guid productId)
        {
            Console.WriteLine($"Serve {productId} at {ServingTemp} temperature.");
        }
    }

    [Table("Product")]
    public class Drink : Product
    {
        public SugarLevel SugarLevel { get; set; }
        public bool IsCaffeinated { get; set; }

        public Drink() : base() { }

        public Drink(string name, string description, decimal price, string image, string ingredients, int stockQuantity, TimeSpan prepTime, ProductStatus status, SugarLevel sugar, bool caffeinated)
            : base(name, description, price, image, ingredients, stockQuantity, prepTime, status)
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