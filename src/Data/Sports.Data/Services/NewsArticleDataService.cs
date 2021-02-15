using Sports.Data.Context;
using Sports.Data.Entities;
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

        public IQueryable<NewsArticle> GetPopularNews(DateTimeOffset fromDate, int newsCount)
        {
            var date = fromDate.UtcDateTime;
            return _sportsContext.NewsArticles
                .Where(x => x.PublishedDate >= date)
                .OrderByDescending(x => x.CommentsCount)
                .Take(newsCount);
        }
    }
}
