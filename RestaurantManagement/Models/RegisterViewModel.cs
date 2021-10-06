using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Models
{
    public class RegisterViewModel
    {
        [Required()]
        public string UserName { get; set; }
        [Required()]
        public string Password { get; set; }
        [Required()]
        public string FullName { get; set; }
        [Required()]
        public bool Gender { get; set; }
        [Required()]
        public DateTime Birthday { get; set; }

    }
}
