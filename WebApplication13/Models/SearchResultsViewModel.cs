using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication13.Models
{
    public class SearchResultsViewModel
    {
        public List<Product> Products { get; set; }
        public string SearchText { get; set; }
    }
}
