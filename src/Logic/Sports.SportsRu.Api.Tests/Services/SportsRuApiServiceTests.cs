using Microsoft.Extensions.Logging;
using Moq;
using Sports.Common.Tests;
using Sports.SportsRu.Api.Models;
using Sports.SportsRu.Api.Resources;
using Sports.SportsRu.Api.Services;
using Sports.SportsRu.Api.Services.Interfaces;
using Sports.SportsRu.Api.Tests.TestsInfrastructure;
using Sports.SportsRu.Api.Tests.TestsInfrastructure.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Sports.SportsRu.Api.Tests.Services
{
    [Collection(TestsConstants.SportsRuApiCollectionName)]
    public class SportsRuApiServiceTests : BaseTests
    {
        private readonly ISportsRuApiService _sportsRuApiService;

        public SportsRuApiServiceTests(SportsRuApiFixture sportsRuApiFixture, ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            _sportsRuApiService = sportsRuApiFixture.SportsRuApiService;
        }

        [Fact]
        public async Task GetNews()
        {
            var newsResponse = await _sportsRuApiService
                .GetNewsAsync(NewsType.HomePage, NewsPriority.Main, NewsContentOrigin.Mixed, 10).ConfigureAwait(false);
            Assert.True(newsResponse.IsSuccess, newsResponse.ErrorMessage);
            Assert.NotNull(newsResponse.Content);
            Assert.NotEmpty(newsResponse.Content);
            foreach (var newsArticle in newsResponse.Content)
            {
                Assert.NotNull(newsArticle.Title);
                Assert.NotNull(newsArticle.DesktopUrl);
                Assert.NotEqual(0, newsArticle.Id);
                Assert.NotNull(newsArticle.Published);
                Assert.NotEqual(0, newsArticle.Published.Timestamp);
                if(newsArticle.ContentOption != null)
                {
                    Assert.NotEmpty(newsArticle.ContentOption.Name);
                }
                Assert.NotNull(newsArticle.Section);
                Assert.NotNull(newsArticle.Section.Name);
            }
            WritePrettyJson(newsResponse.Content);
        }

        [Fact]
        public async Task GetCommentsIds()
        {
            int id = 1084853230; //https://www.sports.ru/football/1084853230.html
            var commentsIdsResponse = await _sportsRuApiService.GetCommentsIdsAsync(id, MessageClass.News, Sort.Top10).ConfigureAwait(false);
            Assert.True(commentsIdsResponse.IsSuccess, commentsIdsResponse.ErrorMessage);
            Assert.NotNull(commentsIdsResponse.Content);
            Assert.NotEmpty(commentsIdsResponse.Content);
            foreach (var item in commentsIdsResponse.Content)
            {

                Assert.NotEqual(0, item);
            }
            WritePrettyJson(commentsIdsResponse.Content);
        }

        [Fact]
        public async Task GetCommentsByIds()
        {
            var commentsIds = new int[] { 1084853975, 1084853842 };//comments ids from https://www.sports.ru/football/1084853230.html
            var commentsByIdsResponse = await _sportsRuApiService.GetCommentsByIds(commentsIds).ConfigureAwait(false);
            Assert.True(commentsByIdsResponse.IsSuccess, commentsByIdsResponse.ErrorMessage);
            Assert.NotNull(commentsByIdsResponse.Content);
            Assert.NotNull(commentsByIdsResponse.Content.Data);
            Assert.NotNull(commentsByIdsResponse.Content.Data.Comments);
            Assert.NotEmpty(commentsByIdsResponse.Content.Data.Comments);
            foreach (var comment in commentsByIdsResponse.Content.Data.Comments)
            {
                Assert.NotEqual(0, comment.Id);
                Assert.NotNull(comment.Text);
                Assert.NotNull(comment.Rating);
            }
            WritePrettyJson(commentsByIdsResponse.Content);
        }

        [Fact]
        public async Task GetHotContent()
        {
            var hotContentResponse = await _sportsRuApiService.GetHotContentAsync().ConfigureAwait(false);
            Assert.True(hotContentResponse.IsSuccess, hotContentResponse.ErrorMessage);
            Assert.NotNull(hotContentResponse.Content.News);
            Assert.NotEmpty(hotContentResponse.Content.News);
            WritePrettyJson(hotContentResponse.Content);
        }

        [Fact]
        public async Task GetHotContent_ErrorResponse()
        {
            var sportsRuApiSettings = new SportsRuApiSettings("https://www.sports.ru", "https://anyvalid.url");
            var sportsRuApiService = new SportsRuApiService(sportsRuApiSettings, Mock.Of<ILogger<SportsRuApiService>>());
            var hotContentResponse = await sportsRuApiService.GetHotContentAsync().ConfigureAwait(false);
            Assert.False(hotContentResponse.IsSuccess);
            Assert.Equal(SportsRuApiResources.Error_Unknown, hotContentResponse.ErrorMessage);
        }
    }
}
