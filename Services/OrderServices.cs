using System.Threading.Tasks;
using DatabaseServices;
using ProjectMaui.Models;

namespace DotnetMauiProject.Services
{
    public class OrderService
    {
        private readonly DatabaseServiceConnection databaseConnect;

        public OrderService(DatabaseServiceConnection db)
        {
            databaseConnect = db;
        }
        public async Task<List<Order>> GetOrdersList()
        {
            var database = await databaseConnect.GetConnection();
            var orders = await database.Table<Order>().ToListAsync();

            foreach (var order in orders)
            {
                var itemsOrder = await database.Table<OrderItem>().Where(x => x.OrderId == order.OrderId).ToListAsync();
                order.OrderItems = itemsOrder;
            }
            return orders;
        }
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