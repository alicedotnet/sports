using Sports.Data.Context;
using Sports.Data.Entities;
using Sports.Services.Interfaces;
using Sports.SportsRu.Api.Models;
using Sports.SportsRu.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports.Services
{
    public class SyncService : ISyncService
    {
        private readonly SportsContext _sportsContext;
        private readonly ISportsRuApiService _sportsRuApiService;

        public SyncService(SportsContext sportsContext, ISportsRuApiService sportsRuApiService)
        {
            _sportsContext = sportsContext;
            _sportsRuApiService = sportsRuApiService;
        }

        public async Task SyncAllAsync()
        {
            var newsResponse = await _sportsRuApiService.GetNewsAsync(NewsType.HomePage, NewsPriority.Main, NewsContentOrigin.Mixed, 10).ConfigureAwait(false);
            if(newsResponse.IsSuccess)
            {
                foreach (var newsArticle in newsResponse.Content)
                {
                    if(newsArticle.BodyIsEmpty ||
                        newsArticle.ContentOption?.Name == "special") //usually this is not a news article but some promotion
                    {
                        continue;
                    }
                    string idString = newsArticle.Id.ToString(CultureInfo.InvariantCulture);
                    if (!_sportsContext.NewsArticles.Any(x => x.ExternalId == idString))
                    {
                        _sportsContext.NewsArticles.Add(new NewsArticle()
                        {
                            ExternalId = idString,
                            Title = newsArticle.Title,
                            Url = newsArticle.DesktopUrl,
                            PublishedDate = DateTimeOffset
                                .FromUnixTimeSeconds(newsArticle.Published.Timestamp)
                                .UtcDateTime
                        });
                    }
                }
                _sportsContext.SaveChanges();
            }
        }

        public void DeleteOldData(DateTimeOffset oldestDateToKeep)
        {
            var date = oldestDateToKeep.UtcDateTime;
            var oldArticles = _sportsContext.NewsArticles
                .Where(x => x.PublishedDate < date).ToArray();
            _sportsContext.NewsArticles.RemoveRange(oldArticles);

            _sportsContext.SaveChanges();
        }
    }
}
