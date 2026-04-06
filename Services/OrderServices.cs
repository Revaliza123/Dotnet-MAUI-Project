using ProjectMaui.Models;

namespace DotnetMauiProject.Services
{
    public class OrderService
    {
        public void UpdateOrderStatus(int orderId, OrderStatus orderStatus)
        {
            Console.WriteLine($"{orderId} orders");
            Console.WriteLine($"{orderStatus} status");
        }
        public void ProccessPayment(int orderId, PaymentStatus paymentStatus)
        {
            Console.WriteLine($"{orderId} orders");
            Console.WriteLine($"{paymentStatus} status");
        }
        public void UpdateOrderItemStatus(int productId, ItemStatus itemStatus)
        {
            Console.WriteLine($"{productId} orders");
            Console.WriteLine($"{itemStatus} status");
        }
    }
}