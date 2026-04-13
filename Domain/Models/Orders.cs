
using ProjectMaui.Domain.Common;
using SQLite;

namespace ProjectMaui.Domain.Models
{
    public class Order
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Indexed]
        public Guid? CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        
        [Indexed]
        public Guid? TableId { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

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