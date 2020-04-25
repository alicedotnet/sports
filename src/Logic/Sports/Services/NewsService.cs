using Sports.Data.Context;
using Sports.Data.Entities;
using Sports.Models;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sports.Services
{
    public class NewsService : INewsService
    {
        private readonly SportsContext _sportsContext;

        public NewsService(SportsContext sportsContext)
        {
            _sportsContext = sportsContext;
        }

        public IEnumerable<NewsArticleModel> GetLatestNews(int newsCount)
        {
            return _sportsContext.NewsArticles
                .OrderByDescending(x => x.PublishedDate)
                .Take(newsCount)
                .Select(x => new NewsArticleModel() 
                { 
                    Title = x.Title,
                    Url = x.Url,
                    CommentsCount = x.CommentsCount,
                    IsHotContent = x.IsHotContent
                })
                .ToArray();
        }
    }
}
