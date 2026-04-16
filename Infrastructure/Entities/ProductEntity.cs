using SQLite;

namespace ProjectMaui.Infrastructure.Entities
{
    [Table("Products")]
    public class ProductEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Ingredients { get; set; }
        public int StockQuantity { get; set; }
        public TimeSpan PreparationTime { get; set; }
        public int Status { get; set; }
        
        public string ProductType { get; set; } 

        public string Taste { get; set; }
        public string NutritionInfo { get; set; }

        public int SweetnessLevel { get; set; }
        public int ServingTemp { get; set; }

        public int SugarLevel { get; set; }
        public bool IsCaffeinated { get; set; }
    }
}