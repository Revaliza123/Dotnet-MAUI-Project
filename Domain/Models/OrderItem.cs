using ProjectMaui.Domain.Common;
using SQLite;

namespace ProjectMaui.Domain.Models;

public class OrderItem
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Indexed]
    public Guid OrderId { get; set; }

    [Indexed]
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Note { get; set; } = default!;
    public ItemStatus ItemStatus { get; set; }
    public decimal SubTotal => Quantity * UnitPrice;

    public OrderItem() { }
    public OrderItem(int quantity, decimal unitPrice, ItemStatus status)
    {
        Quantity = Guard.AtLeast(quantity, 1, nameof(quantity));
        UnitPrice = Guard.NotNegative(unitPrice, nameof(unitPrice));
        ItemStatus = status;
    }
}