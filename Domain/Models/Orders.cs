
using ProjectMaui.Domain.Common;
using SQLite;

namespace ProjectMaui.Domain.Models
{
    public class Order
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Indexed]
        public Guid? CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        
        [Indexed]
        public Guid? TableId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public OrderStatus OrderStatus { get; set; }

        [Ignore]
        public List<OrderItem> OrderItems { get; set; } = new();

        public Order() { }
        public Order(DateTime orderDate, PaymentStatus paymentStatus, OrderStatus status)
        {
            OrderDate = Guard.NotDefault(orderDate, nameof(orderDate));
            PaymentStatus = paymentStatus;
            OrderStatus = status;
        }

        public void CalculateTotalAmount(Guid orderId)
        {
            Console.WriteLine($"{orderId} orders");
        }
    }
}