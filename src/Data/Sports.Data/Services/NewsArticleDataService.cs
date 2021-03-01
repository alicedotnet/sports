using Sports.Data.Context;
using Sports.Data.Entities;
using Sports.Data.Models;
using Sports.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports.Data.Services
{
    public class NewsArticleDataService : INewsArticleDataService
    {
        private readonly SportsContext _sportsContext;

        public NewsArticleDataService(SportsContext sportsContext)
        {
            _sportsContext = sportsContext;
        }

        public IQueryable<NewsArticle> GetPopularNews(
            DateTimeOffset fromDate, SportKind sportKind = SportKind.Undefined)
        {
            var date = fromDate.UtcDateTime;
            IQueryable<NewsArticle> query = _sportsContext.NewsArticles;
            if(sportKind != SportKind.Undefined && sportKind != SportKind.All)
            {
                query = query.Where(x => x.SportKind == sportKind);
            }
            return query
                .Where(x => x.PublishedDate >= date)
                .OrderByDescending(x => x.CommentsCount);
        }
    }
}
