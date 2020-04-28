using Microsoft.EntityFrameworkCore;
using Sports.Data.Context;
using Sports.Services;
using Sports.Services.Interfaces;
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
            builder.UseInMemoryDatabase("sports");
            SportsContext = new SportsContext(builder.Options);

            ISportsRuApiService sportsRuApiService = new SportsRuApiService();
            NewsService = new NewsService(SportsContext);
            SyncService = new SyncService(SportsContext, sportsRuApiService, NewsService);
            NewsArticleCommentService = new NewsArticleCommentService(SportsContext);
        }
    }
}
