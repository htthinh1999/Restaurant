using System;
using Microsoft.AspNetCore.Identity;

namespace RestaurantManagement.Data.Entities
{
    public class OrderTable
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public int TableId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public Customer Customer { get; set; }
        public Table Table { get; set; }
    }
}