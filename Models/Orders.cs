using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectMaui.Models
{
    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Note { get; set; }
        public ItemStatus ItemStatus { get; set; }

        public decimal SubTotal => Quantity * UnitPrice;
    }

    public class Order
    {
        public string OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TableNumber { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}