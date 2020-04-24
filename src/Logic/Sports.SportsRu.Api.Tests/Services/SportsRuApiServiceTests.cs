using Sports.Common.Tests;
using Sports.SportsRu.Api.Models;
using Sports.SportsRu.Api.Services.Interfaces;
using Sports.SportsRu.Api.Tests.TestsInfrastructure;
using Sports.SportsRu.Api.Tests.TestsInfrastructure.Fixtures;
using System.Linq;
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
            foreach (var item in newsResponse.Content)
            {
                Assert.NotNull(item.Title);
                Assert.NotEqual(0, item.Id);
                Assert.NotNull(item.Published);
                Assert.NotEqual(0, item.Published.Timestamp);
                if(item.ContentOption != null)
                {
                    Assert.NotEmpty(item.ContentOption.Name);
                }
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
    }
}
