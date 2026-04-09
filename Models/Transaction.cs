using ProjectMaui.Common;
using SQLite;

namespace ProjectMaui.Models;

public class Transaction
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Indexed]
    public Guid OrderId { get; private set; }

    public decimal AmountPaid { get; private set; }
    public decimal Change { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }

    public Transaction() { }

    public Transaction(Guid orderId, decimal amountPaid, decimal totalBill, PaymentMethod method)
    {
        OrderId = orderId;
        AmountPaid = Guard.Positive(amountPaid, nameof(amountPaid));
        TransactionDate = DateTime.Now;
        PaymentMethod = method;

        if (amountPaid < totalBill)
            throw new ArgumentException("Pembayaran kurang!");

        Change = amountPaid - totalBill;
    }
}