using Sports.Data.Context;
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
    public class SyncServiceTests
    {
        private readonly ISyncService _syncService;
        private readonly SportsContext _sportsContext;

        public SyncServiceTests(SportsFixture sportsFixture)
        {
            _syncService = sportsFixture.SyncService;
            _sportsContext = sportsFixture.SportsContext;
        }

        [Fact]
        public async Task TestSyncAll()
        {
            await _syncService.SyncAllAsync().ConfigureAwait(false);
            Assert.True(_sportsContext.NewsArticles.Any());
        }
    }
}
