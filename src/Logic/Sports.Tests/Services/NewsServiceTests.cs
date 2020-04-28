using Sports.Services.Interfaces;
using Sports.Tests.TestsInfrastructure;
using Sports.Tests.TestsInfrastructure.Fixtures;
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

        public NewsServiceTests(SportsFixture sportsFixture)
        {
            _newsService = sportsFixture.NewsService;
            _syncService = sportsFixture.SyncService;
        }

        [Fact]
        public async Task NextPopularNewsArticle()
        {
            await _syncService.SyncNewsAsync().ConfigureAwait(false);
            var article = _newsService.GetPopularNews(DateTimeOffset.Now.AddDays(-1), 1).First();
            var nextArticle = _newsService.GetNextPopularNewsArticle(article.Id);
            Assert.NotEqual(article.Id, nextArticle.Id);
        }
    }
}
