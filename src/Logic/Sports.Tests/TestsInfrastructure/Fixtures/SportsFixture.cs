using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Sports.Data.Context;
using Sports.Data.Services;
using Sports.Services;
using Sports.Services.Interfaces;
using Sports.SportsRu.Api;
using Sports.SportsRu.Api.Services;
using Sports.SportsRu.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Tests.TestsInfrastructure.Fixtures
{
    public class SportsFixture
    {
        public ISyncService SyncService { get; }
        public INewsService NewsService { get; }
        public INewsArticleCommentService NewsArticleCommentService { get; }
        public SportsContext SportsContext { get; }

        public SportsFixture()
        {
            var builder = new DbContextOptionsBuilder();
            builder
                .UseLazyLoadingProxies()
                .UseSqlite("Data Source=sports.db");
            SportsContext = new SportsContext(builder.Options);
            SportsContext.Database.EnsureDeleted();
            SportsContext.Database.EnsureCreated();

            var sportsRuLogger = Mock.Of<ILogger<SportsRuApiService>>();
            var sportsRuApiSettings = new SportsRuApiSettings("https://www.sports.ru", "https://stat.sports.ru");
            ISportsRuApiService sportsRuApiService = new SportsRuApiService(sportsRuApiSettings, sportsRuLogger);
            var newsArticleDataService = new NewsArticleDataService(SportsContext);
            NewsService = new NewsService(SportsContext, newsArticleDataService);
            var syncServiceLogger = Mock.Of<ILogger<SyncService>>();
            SyncService = new SyncService(SportsContext, sportsRuApiService, newsArticleDataService, syncServiceLogger);
            NewsArticleCommentService = new NewsArticleCommentService(SportsContext);
        }
    }
}
