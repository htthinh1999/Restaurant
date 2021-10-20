using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Models
{
    public class PaymentHistoryViewModel
    {
        public Guid Id { get; set; }
        public int Total { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
