using Sports.Data.Context;
using Sports.Data.Entities;
using Sports.Data.Services.Interfaces;
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
        private readonly INewsArticleDataService _newsDataService;

        public NewsService(SportsContext sportsContext, INewsArticleDataService newsDataService)
        {
            _sportsContext = sportsContext;
            _newsDataService = newsDataService;
        }

        public NewsArticleModel GetById(Guid id)
        {
            var entity = _sportsContext.NewsArticles.FirstOrDefault(x => x.NewsArticleId == id);
            if(entity != null)
            {
                return new NewsArticleModel()
                {
                    Id = entity.NewsArticleId,
                    Title = entity.Title,
                    Url = entity.Url,
                    CommentsCount = entity.CommentsCount,
                    IsHotContent = entity.IsHotContent,
                    ExternalId = entity.ExternalId
                };
            }
            return null;
        }

        public IEnumerable<NewsArticleModel> GetLatestNews(int newsCount, SportKind sportKind = SportKind.Undefined)
        {
            IQueryable<NewsArticle> query = _sportsContext.NewsArticles;
            if(sportKind != SportKind.Undefined && sportKind != SportKind.All)
            {
                query = query.Where(x => x.SportKind == sportKind);
            }
            return query
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

        public IEnumerable<NewsArticleModel> GetPopularNews(DateTimeOffset fromDate, int newsCount, SportKind sportKind = SportKind.Undefined)
        {
            return _newsDataService.GetPopularNews(fromDate, newsCount, sportKind)
                .Select(x => new NewsArticleModel()
                {
                    Id = x.NewsArticleId,
                    Title = x.Title,
                    Url = x.Url,
                    CommentsCount = x.CommentsCount,
                    IsHotContent = x.IsHotContent,
                    ExternalId = x.ExternalId
                })
                .ToArray();
        }

        public NewsArticleModel GetNextPopularNewsArticle(DateTimeOffset fromDate, Guid newsArticleId)
        {
            var date = fromDate.UtcDateTime;
            var newsArticle = _sportsContext.NewsArticles
                .Where(x => x.PublishedDate >= date)
                .OrderByDescending(x => x.CommentsCount)
                .AsEnumerable()
                .SkipWhile(x => x.NewsArticleId != newsArticleId)
                .Skip(1)
                .FirstOrDefault();
            if(newsArticle != null)
            {
                return new NewsArticleModel()
                {
                    Id = newsArticle.NewsArticleId
                };
            }
            return null;
        }
    }
}
