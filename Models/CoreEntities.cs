using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectMaui.Models
{
    public class Table
    {
        public int TableId { get; set; }
        public int? TableNumber { get; set; }
        public string Area { get; set; }
        public int Capacity { get; set; }
        public TableStatus Status { get; set; }
    }
}