using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectMaui.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserRole Role { get; set; }
    }

    public class Table
    {
        public string TableId { get; set; }
        public int? TableNumber { get; set; }
        public string Area { get; set; }
        public int Capacity { get; set; }
        public TableStatus Status { get; set; }
    }

    public class Inventory
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}