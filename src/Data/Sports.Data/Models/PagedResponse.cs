using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports.Data.Models
{
    public class PagedResponse<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }

        public PagedResponse(IEnumerable<T> items, int total)
        {
            Items = items;
            Total = total;
        }
    }
}
