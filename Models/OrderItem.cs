using ProjectMaui.Common;
using SQLite;

namespace ProjectMaui.Models;

public class OrderItem
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string Note { get; private set; }
    public ItemStatus ItemStatus { get; private set; }
    public decimal SubTotal => Quantity * UnitPrice;

    public OrderItem() { }
    public OrderItem(int quantity, decimal unitPrice, ItemStatus status)
    {
        Quantity = Guard.AtLeast(quantity, 1, nameof(quantity));
        UnitPrice = Guard.NotNegative(unitPrice, nameof(unitPrice));
        ItemStatus = status;
    }
}