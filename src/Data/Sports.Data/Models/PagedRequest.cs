using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports.Data.Models
{
    public class PagedRequest
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public PagedRequest(int pageSize, int pageIndex = 0)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
        }
    }
}
