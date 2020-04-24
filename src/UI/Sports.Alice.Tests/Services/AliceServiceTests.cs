using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Sports.Alice.Models.Settings;
using Sports.Alice.Services;
using Sports.Alice.Services.Interfaces;
using Sports.Alice.Tests.TestsInfrastructure;
using Sports.Alice.Tests.TestsInfrastructure.Fixtures;
using Sports.Common.Tests;
using Sports.Data.Context;
using Sports.Services;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Tests.Services
{
    [Collection(TestsConstants.ServerCollectionName)]
    public class AliceServiceTests : BaseTests
    {
        private readonly IAliceService _aliceService;
        private readonly SportsSettings _sportsSettings;

        public AliceServiceTests(MockContextFixture mockContextFixture, ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            var scope = mockContextFixture.Services.CreateScope();
            _sportsSettings = scope.ServiceProvider.GetService<IOptions<SportsSettings>>().Value;
            _aliceService = scope.ServiceProvider.GetService<IAliceService>();
        }

        [Fact]
        public void GetNews()
        {
            AliceRequest aliceRequest = new AliceRequest()
            {
                Session = new AliceSessionModel(),
                Request = new AliceRequestModel()
                {
                    Nlu = new AliceNLUModel()
                    {
                        Tokens = new string[] {"новости"}
                    }
                }
            };
            var aliceResponse = _aliceService.ProcessRequest(aliceRequest);
            TestOutputHelper.WriteLine($"Response text: {aliceResponse.Response.Text}");
            var news = aliceResponse.Response.Text.Split("\n\n");
            Assert.Equal(_sportsSettings.NewsToDisplay, news.Length);
        }

        [Fact]
        public void InvalidObject()
        {
            AliceRequest aliceRequest = null;
            var exception = Assert.Throws<ArgumentNullException>(() => _aliceService.ProcessRequest(aliceRequest));
            Assert.Equal(nameof(aliceRequest), exception.ParamName);
        }
    }
}
