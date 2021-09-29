using System.Collections.Generic;

namespace RestaurantManagement.Data.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PeopleCount { get; set; }
        public List<Order_table> Order_table { get; set; }
    }
}
