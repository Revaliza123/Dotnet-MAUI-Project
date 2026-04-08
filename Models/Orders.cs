
using SQLite;

namespace ProjectMaui.Models
{
    public class Order
    {
        [PrimaryKey] [AutoIncrement]
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TableNumber { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public OrderStatus OrderStatus { get; set; }
        [Ignore]
        public List<OrderItem> OrderItems { get; set; } = new();

        public void CalculateTotalAmount(int OrderId)
        {
            Console.WriteLine($"{OrderId} orders");
        }
    }
    public class OrderItem
    {
        [PrimaryKey] [AutoIncrement]
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Note { get; set; }
        public ItemStatus ItemStatus { get; set; }
        public decimal SubTotal => Quantity * UnitPrice;
    }

}