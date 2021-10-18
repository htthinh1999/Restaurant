using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Models
{
    public class TableHistoryViewModels
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string TableName { get; set; }
        public int PeopleCount { get; set; }

    }
}
