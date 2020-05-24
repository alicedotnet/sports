using Sports.Data.Context;
using Sports.Services.Interfaces;
using Sports.Tests.TestsInfrastructure;
using Sports.Tests.TestsInfrastructure.Fixtures;
using Sports.Tests.TestsInfrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sports.Tests.Services
{
    [Collection(TestsConstants.SportsCollectionName)]
    public class NewsServiceTests
    {
        private readonly INewsService _newsService;
        private readonly ISyncService _syncService;
        private readonly SportsContext _sportsContext;

        public NewsServiceTests(SportsFixture sportsFixture)
        {
            _newsService = sportsFixture.NewsService;
            _syncService = sportsFixture.SyncService;
            _sportsContext = sportsFixture.SportsContext;
        }

        [Fact]
        public async Task NextPopularNewsArticle()
        {
            await _syncService.SyncNewsAsync().ConfigureAwait(false);
            var fromDate = DateTimeOffset.Now.AddDays(-1);
            var article = _newsService.GetPopularNews(fromDate, 1).First();
            var nextArticle = _newsService.GetNextPopularNewsArticle(fromDate, article.Id);
            Assert.NotEqual(article.Id, nextArticle.Id);
            SportsContextHelper.DeleteAllNews(_sportsContext);
        }
    }
}
