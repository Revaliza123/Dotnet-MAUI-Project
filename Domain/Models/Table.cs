using System;
using System.Collections.Generic;
using System.Linq;
using ProjectMaui.Domain.Common;
using SQLite;

namespace ProjectMaui.Domain.Models
{
    public class Table
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public int? TableNumber { get; private set; }
        public string Area { get; private set; }
        public int Capacity { get; private set; }
        public TableStatus Status { get; private set; }
        public Table() { }
        public Table(int tableNumber, string area, int capacity, TableStatus status)
        {
            TableNumber = Guard.AtLeast(tableNumber, 1, nameof(tableNumber));
            Area = Guard.NotNullOrWhiteSpace(area, nameof(area));
            Capacity = Guard.AtLeast(capacity, 2, nameof(capacity));
            Status = status;
        }
    }
}