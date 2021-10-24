using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Models
{
    public class PaymentViewModel
    {
        public Guid CustomerId { get; set; }
        public Guid BillId { get; set; }
        public string FoodName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Total { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
