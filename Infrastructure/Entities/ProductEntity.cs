using SQLite;

namespace ProjectMaui.Infrastructure.Entities
{
    [Table("Products")]
    public class ProductEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public Guid? CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; } = default!;
        public string Ingredients { get; set; } = default!;
        public int StockQuantity { get; set; }
        public TimeSpan PreparationTime { get; set; }
        public int Status { get; set; }
        public string ProductType { get; set; } = default!;
        public string Taste { get; set; } = default!;
        public string NutritionInfo { get; set; } = default!;

        public int SweetnessLevel { get; set; }
        public int ServingTemp { get; set; }

        public int SugarLevel { get; set; }
        public bool IsCaffeinated { get; set; }
    }
}