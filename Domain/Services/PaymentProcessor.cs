using ProjectMaui.Domain.Infrasturcture;
using ProjectMaui.Domain.Models;
using SQLite;

namespace ProjectMaui.Domain.Services
{
    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly DatabaseService _databaseService;
        private SQLiteAsyncConnection? _connection;

        public PaymentProcessor(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        private async Task<SQLiteAsyncConnection> GetDb()
        {
            if (_connection == null)
                _connection = await _databaseService.GetConnection();
            return _connection;
        }

        public async Task<PaymentStatus> ProcessPaymentAsync(Guid orderId, decimal amountPaid, decimal totalBill, PaymentMethod method)
        {
            var transaction = new Transaction(orderId, amountPaid, totalBill, method);
            var db = await GetDb();
            await db.InsertAsync(transaction);

            var order = await db.Table<Order>().FirstOrDefaultAsync(o => o.Id == orderId);
            if (order != null)
            {
                order.PaymentStatus = PaymentStatus.Success;
                await db.UpdateAsync(order);
            }

            return PaymentStatus.Success;
        }

        public async Task<PaymentStatus> RefundAsync(Guid transactionId)
        {
            var db = await GetDb();
            var transaction = await db.Table<Transaction>().FirstOrDefaultAsync(t => t.Id == transactionId);
            if (transaction == null)
                return PaymentStatus.Failed;

            var order = await db.Table<Order>().FirstOrDefaultAsync(o => o.Id == transaction.OrderId);
            if (order != null)
            {
                order.PaymentStatus = PaymentStatus.Refunded;
                await db.UpdateAsync(order);
            }

            return PaymentStatus.Refunded;
        }

        public async Task<Transaction?> GetTransactionAsync(Guid transactionId)
        {
            var db = await GetDb();
            return await db.Table<Transaction>().FirstOrDefaultAsync(t => t.Id == transactionId);
        }
    }
}
