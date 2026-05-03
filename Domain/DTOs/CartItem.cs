using ProjectMaui.Domain.Models;

namespace ProjectMaui.Domain.DTOs;
public class CartItem
{
    public Product Product { get; set; } = default!;
    public int Quantity { get; set; }
    public string Note { get; set; } = string.Empty;
    public SugarLevel SugarLevel { get; set; }
    public ServingTemp ServingTemp { get; set; }
    public decimal UnitPrice => Product.Price;
    public decimal SubTotal => UnitPrice * Quantity;

    public CartItem(Product product)
    {
        Product = product;
        Quantity = 1;
    }
}
