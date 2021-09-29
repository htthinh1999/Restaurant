using System.Collections.Generic;

namespace RestaurantManagement.Data.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PeopleCount { get; set; }

        public List<OrderTable> OrderTables { get; set; }
    }
}
