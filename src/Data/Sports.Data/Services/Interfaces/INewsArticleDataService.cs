using Sports.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports.Data.Services.Interfaces
{
    public interface INewsArticleDataService
    {
        IQueryable<NewsArticle> GetPopularNews(DateTimeOffset fromDate, int newsCount);
    }
}
