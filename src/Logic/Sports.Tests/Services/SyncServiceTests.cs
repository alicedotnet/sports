using Sports.Common.Tests;
using Sports.Data.Context;
using Sports.Data.Entities;
using Sports.Services;
using Sports.Services.Interfaces;
using Sports.SportsRu.Api.Models;
using Sports.Tests.TestsInfrastructure;
using Sports.Tests.TestsInfrastructure.Fixtures;
using Sports.Tests.TestsInfrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            Assert.Empty(_sportsContext.NewsArticles);
            await _syncService.SyncNewsAsync().ConfigureAwait(false);
            Assert.True(_sportsContext.NewsArticles.Any());
            var article = _sportsContext.NewsArticles.First();
            Assert.NotNull(article.ExternalId);
            Assert.NotNull(article.PublishedDate);
            Assert.True(_sportsContext.NewsArticles.Any(x => x.SportKind != SportKind.Undefined));

            SportsContextHelper.DeleteAllNews(_sportsContext);
        }

        [Fact]
        public async Task SyncComments()
        {
            Assert.Empty(_sportsContext.NewsArticlesComments);
            await _syncService.SyncNewsAsync().ConfigureAwait(false);
            await _syncService.SyncPopularNewsCommentsAsync(DateTimeOffset.Now.AddDays(-1), 10).ConfigureAwait(false);
            Assert.NotEmpty(_sportsContext.NewsArticlesComments);

            SportsContextHelper.DeleteAllNews(_sportsContext);
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

            SportsContextHelper.DeleteAllNews(_sportsContext);
        }

        [Fact]
        public void DeleteAfterDate()
        {
            var newsArticle = new NewsArticle()
            { Title = "test", PublishedDate = DateTime.Now.AddDays(-1) };
            _sportsContext.NewsArticles.Add(newsArticle);

            Assert.NotEqual(Guid.Empty, newsArticle.NewsArticleId);
            _sportsContext.NewsArticlesComments.Add(new NewsArticleComment()
            {
                NewsArticleId = newsArticle.NewsArticleId,
                Text = string.Empty
            });

            _sportsContext.SaveChanges();

            Assert.True(_sportsContext.NewsArticlesComments.Any());
            Assert.True(_sportsContext.NewsArticles.Any());
            _syncService.DeleteOldData(DateTimeOffset.Now);
            Assert.False(_sportsContext.NewsArticlesComments.Any());
            Assert.False(_sportsContext.NewsArticles.Any());
        }

        [Fact]
        public void SyncService_Map_ReplaceQuot()
        {
            Type type = typeof(SyncService);
            //var hello = Activator.CreateInstance(type, firstName, lastName);
            var syncService = new SyncService(null, null, null, null);
            MethodInfo mapMethod = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(x => x.Name == "Map" && x.IsPrivate)
                .First();

            string initialText = "&quot;это&quot;";
            CommentInfo from = new CommentInfo()
            {
                Rating = new CommentRating(),
                Text = initialText
            };
            NewsArticleComment to = new NewsArticleComment();
            mapMethod.Invoke(syncService, new object[] { from, to });
            Assert.NotEqual(from.Text, to.Text);
            string mappedText = "\"это\"";
            Assert.Equal(mappedText, to.Text);
        }
    }
}
