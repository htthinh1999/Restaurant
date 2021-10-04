using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Data.Entities
{
    public class BillDetail
    {
        public Guid BillId { get; set; }
        public int FoodId { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }

        public Food Food { get; set; }
        public Bill Bill { get; set; }
    }
}
