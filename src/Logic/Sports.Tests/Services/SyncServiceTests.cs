using Sports.Common.Tests;
using Sports.Data.Context;
using Sports.Data.Entities;
using Sports.Services.Interfaces;
using Sports.Tests.TestsInfrastructure;
using Sports.Tests.TestsInfrastructure.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Sports.Tests.Services
{
    [Collection(TestsConstants.SportsCollectionName)]
    public class SyncServiceTests : BaseTests
    {
        private readonly ISyncService _syncService;
        private readonly SportsContext _sportsContext;

        public SyncServiceTests(SportsFixture sportsFixture, ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            _syncService = sportsFixture.SyncService;
            _sportsContext = sportsFixture.SportsContext;
        }

        [Fact]
        public async Task TestSyncAll()
        {
            await _syncService.SyncAllAsync().ConfigureAwait(false);
            Assert.True(_sportsContext.NewsArticles.Any());
            var article = _sportsContext.NewsArticles.First();
            Assert.NotNull(article.ExternalId);
            Assert.NotNull(article.PublishedDate);

            WritePrettyJson(article);
        }

        [Fact]
        public void DeleteAfterDate()
        {
            Assert.False(_sportsContext.NewsArticles.Any());
            _sportsContext.NewsArticles.Add(new NewsArticle() 
                { Title = "test", PublishedDate = DateTime.Now.AddDays(-1) });
            _sportsContext.SaveChanges();
            Assert.True(_sportsContext.NewsArticles.Any());
            _syncService.DeleteOldData(DateTimeOffset.Now);
            Assert.False(_sportsContext.NewsArticles.Any());
        }
    }
}
