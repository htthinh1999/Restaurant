using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Data.Entities
{
    public class Bill
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public int Total { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedDate { get; set; }

        public Customer Customer { get; set; }
        public List<BillDetail> BillDetails { get; set; }
    }
}
