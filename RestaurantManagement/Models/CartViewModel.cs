using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Models
{
    public class CartViewModel
    {
        public int Total { get; set; }
        public List<CartDetailViewModel> ListFood { get; set; }
    }
}
