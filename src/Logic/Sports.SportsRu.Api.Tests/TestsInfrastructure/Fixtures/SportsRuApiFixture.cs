using Microsoft.Extensions.Logging;
using Moq;
using Sports.SportsRu.Api.Services;
using Sports.SportsRu.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.SportsRu.Api.Tests.TestsInfrastructure.Fixtures
{
    public class SportsRuApiFixture
    {
        public ISportsRuApiService SportsRuApiService { get; }
        public SportsRuApiFixture()
        {
            var logger = Mock.Of<ILogger<SportsRuApiService>>();
            var sportsRuApiSettings = new SportsRuApiSettings("https://www.sports.ru", "https://stat.sports.ru");
            SportsRuApiService = new SportsRuApiService(sportsRuApiSettings, logger);
        }
    }
}
