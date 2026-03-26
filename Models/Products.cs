using System;
using System.Collections.Generic;

namespace ProjectMaui.Models
{
    public abstract class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public TimeSpan PreparationTime { get; set; }
        public ProductStatus Status { get; set; }
    }

    public class Food : Product
    {
        public List<string> Ingredients { get; set; } = new();
        public string Taste { get; set; }
        public string NutritionInfo { get; set; }
    }

    public class Dessert : Food
    {
        public int SweetnessLevel { get; set; }
        public ServingTemp ServingTemp { get; set; }
    }

    public class Drink : Product
    {
        public List<string> Ingredients { get; set; } = new();
        public SugarLevel SugarLevel { get; set; }
        public bool IsCaffeinated { get; set; }
    }
}