using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports.Data.Models
{
    public class PagedRequest
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public PagedRequest(int pageSize)
        {
            CurrentPage = 0;
            PageSize = pageSize;
        }
    }
}
