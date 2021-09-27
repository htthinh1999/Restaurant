using System;
using Microsoft.AspNetCore.Identity;

namespace RestaurantManagement.Data.Entities
{
    public class Customer : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public DateTime Birthday { get; set; }
        public bool VIP { get; set; }
    }
}
