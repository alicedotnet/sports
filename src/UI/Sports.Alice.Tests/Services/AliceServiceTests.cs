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
        private readonly SportsSettings _sportsSettings;
        private readonly IServiceProvider _services;

        public AliceServiceTests(ServerFixture serverFixture, ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            _services = serverFixture.Services;
            _sportsSettings = _services.GetService<IOptions<SportsSettings>>().Value;
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
            using var scope = _services.CreateScope();
            var aliceService = scope.ServiceProvider.GetService<IAliceService>();
            var aliceResponse = aliceService.ProcessRequest(aliceRequest);
            var aliceGalleryResponse = aliceResponse as AliceGalleryResponse;
            Assert.NotNull(aliceGalleryResponse);
            TestOutputHelper.WriteLine($"Response text: {aliceGalleryResponse.Response.Text}");
            Assert.Equal(_sportsSettings.NewsToDisplay, aliceGalleryResponse.Response.Card.Items.Count);
        }

        [Fact]
        public void InvalidObject()
        {
            AliceRequest aliceRequest = null;
            using var scope = _services.CreateScope();
            var aliceService = scope.ServiceProvider.GetService<IAliceService>();
            var exception = Assert.Throws<ArgumentNullException>(() => aliceService.ProcessRequest(aliceRequest));
            Assert.Equal(nameof(aliceRequest), exception.ParamName);
        }
    }
}
