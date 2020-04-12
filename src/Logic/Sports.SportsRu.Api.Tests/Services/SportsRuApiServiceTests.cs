using Sports.SportsRu.Api.Models;
using Sports.SportsRu.Api.Services.Interfaces;
using Sports.SportsRu.Api.Tests.TestsInfrastructure;
using Sports.SportsRu.Api.Tests.TestsInfrastructure.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Sports.SportsRu.Api.Tests.Services
{
    [Collection(TestsConstants.SportsRuApiCollectionName)]
    public class SportsRuApiServiceTests
    {
        private readonly ISportsRuApiService _sportsRuApiService;
        private readonly ITestOutputHelper _testOutputHelper;

        public SportsRuApiServiceTests(SportsRuApiFixture sportsRuApiFixture, ITestOutputHelper testOutputHelper)
        {
            _sportsRuApiService = sportsRuApiFixture.SportsRuApiService;
            _testOutputHelper = testOutputHelper;
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
            }
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(newsResponse.Content, new JsonSerializerOptions() { WriteIndented = true }));
        }

        [Fact]
        public async Task GetCommentsIds()
        {
            var newsResponse = await _sportsRuApiService
                .GetNewsAsync(NewsType.HomePage, NewsPriority.Main, NewsContentOrigin.Mixed, 10).ConfigureAwait(false);
            var commentsResponse = await _sportsRuApiService.GetCommentsIdsAsync(newsResponse.Content.First().Id, MessageClass.News, Sort.Top10);
            Assert.True(commentsResponse.IsSuccess, commentsResponse.ErrorMessage);
            Assert.NotNull(commentsResponse.Content);
            Assert.NotEmpty(commentsResponse.Content);
            foreach (var item in commentsResponse.Content)
            {
                Assert.NotEqual(0, item);
            }
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(commentsResponse.Content, new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
}
