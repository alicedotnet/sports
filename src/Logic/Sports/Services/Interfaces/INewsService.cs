using Sports.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Services.Interfaces
{
    public interface INewsService
    {
        NewsArticleModel GetById(Guid id);
        IEnumerable<NewsArticleModel> GetLatestNews(int newsCount);
        IEnumerable<NewsArticleModel> GetPopularNews(DateTimeOffset fromDate, int newsCount);
        NewsArticleModel GetNextPopularNewsArticle(DateTimeOffset fromDate, Guid newsArticleId);
    }
}
