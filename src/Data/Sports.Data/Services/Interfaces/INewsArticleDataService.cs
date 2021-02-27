using Sports.Data.Entities;
using Sports.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports.Data.Services.Interfaces
{
    public interface INewsArticleDataService
    {
        IQueryable<NewsArticle> GetPopularNews(
            DateTimeOffset fromDate, PagedRequest pagedRequest, SportKind sportKind = SportKind.Undefined);
    }
}
