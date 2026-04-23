using System.Threading.Tasks;
using ProjectMaui.Domain.Infrasturcture;
using ProjectMaui.Domain.Models;
using SQLite;

namespace ProjectMaui.Domain.Services
{
    public class OrderService
    {
        private readonly DatabaseService databaseConnect;
        private SQLiteAsyncConnection? connection;

        public OrderService(DatabaseService db)
        {
            databaseConnect = db;
        }

        private async Task<SQLiteAsyncConnection> GetDb()
        {
            if (connection == null)
            {
                connection = await databaseConnect.GetConnection();
            }
            return connection;
        }

        public async Task AddOrderAsync(Order order)
        {
            var db = await GetDb();

            await db.InsertAsync(order);

            if (order.OrderItems != null && order.OrderItems.Any())
            {
                foreach (var item in order.OrderItems)
                {
                    item.OrderId = order.Id;
                    await db.InsertAsync(item);
                }
            }
        }

        public async Task<List<Order>> GetOrdersWithItemsAsync()
        {
            var db = await GetDb();
            var orders = await db.Table<Order>().ToListAsync();

            foreach (var order in orders)
            {
                var items = await db.Table<OrderItem>().Where(i => i.OrderId == order.Id).ToListAsync();
                order.OrderItems = items;
            }

            return orders;
        }

        public async Task DeleteOrderAsync(Order order)
        {
            var db = await GetDb();

            var items = await db.Table<OrderItem>().Where(i => i.OrderId == order.Id).ToListAsync();
            foreach (var item in items)
            {
                await db.DeleteAsync(item);
            }

            await db.DeleteAsync(order);
        }

        public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus orderStatus)
        {
            var db = await GetDb();
            var order = await db.Table<Order>().FirstOrDefaultAsync(o => o.Id == orderId);

            if (order != null)
            {
                order.OrderStatus = orderStatus;

                await db.UpdateAsync(order);
                Console.WriteLine($"Order {orderId} status updated to {orderStatus}");
            }
        }
        public void ProccessPayment(Guid orderId, PaymentStatus paymentStatus)
        {
            Console.WriteLine($"{orderId} orders");
            Console.WriteLine($"{paymentStatus} status");
        }

        public void UpdateOrderItemStatus(Guid productId, ItemStatus itemStatus)
        {
            Console.WriteLine($"{productId} product");
            Console.WriteLine($"{itemStatus} status");
        }
    }
}