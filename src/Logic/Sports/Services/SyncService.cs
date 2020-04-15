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
                foreach (var item in newsResponse.Content)
                {
                    string idString = item.Id.ToString(CultureInfo.InvariantCulture);
                    if (!_sportsContext.NewsArticles.Any(x => x.ExternalId == idString))
                    {
                        _sportsContext.NewsArticles.Add(new NewsArticle()
                        {
                            ExternalId = idString,
                            Title = item.Title
                        });
                    }
                }
                _sportsContext.SaveChanges();
            }
        }
    }
}
