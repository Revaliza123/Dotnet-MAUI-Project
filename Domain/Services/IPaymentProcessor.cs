using ProjectMaui.Domain.Models;

namespace ProjectMaui.Domain.Services
{
    public interface IPaymentProcessor
    {
        Task<PaymentStatus> ProcessPaymentAsync(Guid orderId, decimal amountPaid, decimal totalBill, PaymentMethod method);
        Task<PaymentStatus> RefundAsync(Guid transactionId);
        Task<Transaction?> GetTransactionAsync(Guid transactionId);
    }
}
