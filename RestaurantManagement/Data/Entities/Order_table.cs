using System;
using Microsoft.AspNetCore.Identity;

namespace RestaurantManagement.Data.Entities
{
    public class Order_table
    {
        public int ID { get; set; }
        public Customer Customer { get; set; }
        public Table Table { get; set; }
        public Guid CustomerID { get; set; }

        public int TableID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}