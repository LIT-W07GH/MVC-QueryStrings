using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication13.Models
{
    public class OrdersViewModel
    {
        public List<Order> Orders { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}
