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
        private readonly INewsService _newsService;
        private readonly INewsArticleCommentService _newsArticleCommentService;
        private readonly SportsContext _sportsContext;

        public SyncServiceTests(SportsFixture sportsFixture, ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            _syncService = sportsFixture.SyncService;
            _newsService = sportsFixture.NewsService;
            _newsArticleCommentService = sportsFixture.NewsArticleCommentService;
            _sportsContext = sportsFixture.SportsContext;
        }

        [Fact]
        public async Task SyncNews()
        {
            await _syncService.SyncNewsAsync().ConfigureAwait(false);
            Assert.True(_sportsContext.NewsArticles.Any());
            var article = _sportsContext.NewsArticles.First();
            Assert.NotNull(article.ExternalId);
            Assert.NotNull(article.PublishedDate);

            WritePrettyJson(article);
        }

        [Fact]
        public async Task SyncComments()
        {
            Assert.Empty(_sportsContext.NewsArticlesComments);
            await _syncService.SyncNewsAsync().ConfigureAwait(false);
            await _syncService.SyncPopularNewsCommentsAsync(DateTimeOffset.Now.AddDays(-1), 1).ConfigureAwait(false);
            Assert.NotEmpty(_sportsContext.NewsArticlesComments);
        }

        [Fact]
        public async Task GetNewsArticlesComments()
        {
            await _syncService.SyncNewsAsync().ConfigureAwait(false);
            await _syncService.SyncPopularNewsCommentsAsync(DateTimeOffset.Now.AddDays(-1), 1).ConfigureAwait(false);
            var popularNewsArticle = _newsService.GetPopularNews(DateTimeOffset.Now.AddDays(-1), 1).First();
            var comments = _newsArticleCommentService.GetBestComments(popularNewsArticle.Id, 5);
            Assert.NotEmpty(comments);
            WritePrettyJson(comments);
        }

        [Fact]
        public void DeleteAfterDate()
        {
            _sportsContext.NewsArticles.Add(new NewsArticle() 
                { Title = "test", PublishedDate = DateTime.Now.AddDays(-1) });
            _sportsContext.SaveChanges();
            Assert.True(_sportsContext.NewsArticles.Any());
            _syncService.DeleteOldData(DateTimeOffset.Now);
            Assert.False(_sportsContext.NewsArticles.Any());
        }
    }
}
