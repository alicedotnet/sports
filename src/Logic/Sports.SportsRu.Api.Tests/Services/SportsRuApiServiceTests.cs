using Sports.SportsRu.Api.Models;
using Sports.SportsRu.Api.Services.Interfaces;
using Sports.SportsRu.Api.Tests.TestsInfrastructure;
using Sports.SportsRu.Api.Tests.TestsInfrastructure.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sports.SportsRu.Api.Tests.Services
{
    [Collection(TestsConstants.SportsRuApiCollectionName)]
    public class SportsRuApiServiceTests
    {
        private ISportsRuApiService _sportsRuApiService;

        public SportsRuApiServiceTests(SportsRuApiFixture sportsRuApiFixture)
        {
            _sportsRuApiService = sportsRuApiFixture.SportsRuApiService;
        }

        [Fact]
        public async Task GetNews()
        {
            var news = await _sportsRuApiService
                .GetNewsAsync(NewsType.HomePage, NewsPriority.Main, NewsContentOrigin.Mixed, 10).ConfigureAwait(false);
            Assert.True(news.IsSuccess, news.ErrorMessage);
            Assert.NotNull(news.Content);
            Assert.NotEmpty(news.Content);
            foreach (var item in news.Content)
            {
                Assert.NotNull(item.Title);
                Assert.NotEqual(0, item.Id);
            }
        }
    }
}
