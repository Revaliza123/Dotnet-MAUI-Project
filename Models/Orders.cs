
using ProjectMaui.Common;
using SQLite;

namespace ProjectMaui.Models
{
    public class Order
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public Guid? TableId { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        [Ignore]
        public List<OrderItem> OrderItems { get; set; } = new();

        public Order() { }
        public Order(DateTime orderDate, PaymentMethod paymentMethod, PaymentStatus paymentStatus, OrderStatus status)
        {
            OrderDate = Guard.NotDefault(orderDate, nameof(orderDate));
            PaymentMethod = paymentMethod;
            PaymentStatus = paymentStatus;
            OrderStatus = status;
        }

        public void CalculateTotalAmount(Guid orderId)
        {
            Console.WriteLine($"{orderId} orders");
        }
    }
}