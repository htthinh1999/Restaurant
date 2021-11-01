using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Models
{
    public class InsertFoodViewModel
    {
        [Required()]
        public Guid CustomerId { get; set; }
        [Required()]
        public Guid BillId { get; set; }
        [Required()]
        public int FoodId { get; set; }
        [Required()]
        public int UnitPrice { get; set; }
        [Required()]
        public int Quantity { get; set; }
        [Required()]
        public int Price { get; set; }
    }
}
