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

        public async Task<List<Order>> GetOrdersList()
        {
            var database = await GetDb();
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
            try
            {
                var db = await GetDb();
                int result = await db.InsertAsync(order);

                if (result > 0)
                {
                    Console.WriteLine($"Succes to add the new data");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Error {exc.Message} when get the data");
                throw;
            }
        }

        public async Task UpdateOrder(Order order)
        {
            try
            {
                var db = await GetDb();
                int result = await db.UpdateAsync(order);

                if (result > 0)
                {
                    Console.WriteLine($"Succes to update the data");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Error {exc.Message} when update the data");
                throw;
            }
        }

        public async Task DeleteOrder(Guid ordeId)
        {
            var db = await GetDb();
            await db.DeleteAsync(ordeId);
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