using System.Threading.Tasks;
using ProjectMaui.Models;
using ProjectMaui.Services;

namespace DotnetMauiProject.Services
{
    public class OrderService
    {
        private readonly DatabaseService databaseConnect;

        public OrderService(DatabaseService db)
        {
            databaseConnect = db;
        }
        public async Task<List<Order>> GetOrdersList()
        {
            var database = await databaseConnect.GetConnection();
            var orders = await database.Table<Order>().ToListAsync();

            foreach (var order in orders)
            {
                var itemsOrder = await database.Table<OrderItem>().Where(x => x.OrderId == order.Id).ToListAsync();
                order.OrderItems = itemsOrder;
            }
            return orders;
        }

        public async Task AddOrder(Order order)
        {
            try {
                var db = await databaseConnect.GetConnection();
                int result = await db.InsertAsync(order);

                if (result > 0) {
                    Console.WriteLine($"Succes to add the new data");
                }
            }
            catch (Exception exc) {
                Console.WriteLine($"Error {exc.Message} when get the data");
                throw;
            }
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