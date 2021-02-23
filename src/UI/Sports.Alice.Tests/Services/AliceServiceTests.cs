using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Sports.Alice.Models;
using Sports.Alice.Models.Settings;
using Sports.Alice.Scenes;
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
            _sportsSettings = _services.GetService<SportsSettings>();
        }

        [Fact]
        public void GetNews()
        {
            var aliceRequest = new SportsRequest()
            {
                Session = new AliceSessionModel(),
                Request = new AliceRequestModel<SportsIntents>()
                {
                    Nlu = new AliceNLUModel<SportsIntents>()
                    {
                        Intents = new SportsIntents()
                        {
                            Read = new AliceIntentModel<ReadSlots>()
                            {
                                Slots = new ReadSlots()
                                {
                                    InfoType = new AliceEntityInfoTypeModel()
                                    {
                                        Value = InfoType.News
                                    },
                                    InfoCategory = new AliceEntityInfoCategoryModel()
                                    {
                                        Value = InfoCategory.Latest
                                    }
                                }
                            }
                        }
                    }
                },
                State = new AliceStateModel<SportsSessionState, object>()
                {
                    Session = new SportsSessionState()
                }
            };
            using var scope = _services.CreateScope();
            var aliceService = scope.ServiceProvider.GetService<IAliceService>();
            var aliceResponse = aliceService.ProcessRequest(aliceRequest);
            Assert.IsType<SportsGalleryResponse>(aliceResponse);
            var aliceGalleryResponse = aliceResponse as SportsGalleryResponse;
            Assert.NotNull(aliceGalleryResponse);
            TestOutputHelper.WriteLine($"Response text: {aliceGalleryResponse.Response.Text}");
            Assert.Equal(_sportsSettings.NewsToDisplay, aliceGalleryResponse.Response.Card.Items.Count);
        }

        [Fact]
        public void GetMainNews()
        {
            var aliceRequest = new SportsRequest()
            {
                Session = new AliceSessionModel(),
                Request = new AliceRequestModel<SportsIntents>()
                {
                    Nlu = new AliceNLUModel<SportsIntents>()
                    {
                        Intents = new SportsIntents()
                        {
                            Read = new AliceIntentModel<ReadSlots>()
                            {
                                Slots = new ReadSlots()
                                {
                                    InfoCategory = new AliceEntityInfoCategoryModel()
                                    {
                                        Value = InfoCategory.Main
                                    },
                                    InfoType = new AliceEntityInfoTypeModel()
                                    {
                                        Value = InfoType.News
                                    }
                                }
                            }
                        }
                    }
                },
                State = new AliceStateModel<SportsSessionState, object>()
                {
                    Session = new SportsSessionState()
                }
            };
            using var scope = _services.CreateScope();
            var aliceService = scope.ServiceProvider.GetService<IAliceService>();
            var aliceResponse = aliceService.ProcessRequest(aliceRequest);
            Assert.IsType<SportsGalleryResponse>(aliceResponse);
            var aliceGalleryResponse = aliceResponse as SportsGalleryResponse;
            Assert.NotNull(aliceGalleryResponse);
            TestOutputHelper.WriteLine($"Response text: {aliceGalleryResponse.Response.Text}");
            Assert.Equal(_sportsSettings.NewsToDisplay, aliceGalleryResponse.Response.Card.Items.Count);
        }

        [Fact]
        public void GetBestComments()
        {
            var aliceRequest = new SportsRequest()
            {
                Session = new AliceSessionModel(),
                Request = new AliceRequestModel<SportsIntents>()
                {
                    Nlu = new AliceNLUModel<SportsIntents>()
                    {
                        Intents = new SportsIntents()
                        {
                            Read = new AliceIntentModel<ReadSlots>()
                            {
                                Slots = new ReadSlots()
                                {
                                    InfoType = new AliceEntityInfoTypeModel()
                                    {
                                        Value = InfoType.Comments
                                    }
                                }
                            }
                        }
                    }
                },
                State = new AliceStateModel<SportsSessionState, object>()
                {
                    Session = new SportsSessionState()
                }
            };
            using var scope = _services.CreateScope();
            var aliceService = scope.ServiceProvider.GetService<IAliceService>();
            var aliceResponse = aliceService.ProcessRequest(aliceRequest);
            Assert.IsType<SportsResponse>(aliceResponse);
            var aliceGalleryResponse = aliceResponse as SportsResponse;
            Assert.NotNull(aliceGalleryResponse);
            TestOutputHelper.WriteLine($"Response text: {aliceGalleryResponse.Response.Text}");
        }

        [Fact]
        public void Fallback()
        {
            var aliceRequest = new SportsRequest()
            {
                Session = new AliceSessionModel(),
                Request = new AliceRequestModel<SportsIntents>()
                {
                    Nlu = new AliceNLUModel<SportsIntents>()
                    {
                        Intents = new SportsIntents()
                        {
                        },
                    },
                    OriginalUtterance = "тест"
                },
                State = new AliceStateModel<SportsSessionState, object>()
                {
                    Session = new SportsSessionState()
                    {
                        CurrentScene = SceneType.MainNews
                    }
                }
            };
            using var scope = _services.CreateScope();
            var aliceService = scope.ServiceProvider.GetService<IAliceService>();
            var aliceResponse = aliceService.ProcessRequest(aliceRequest);
            Assert.IsType<SportsResponse>(aliceResponse);
            var aliceGalleryResponse = aliceResponse as SportsResponse;
            Assert.NotNull(aliceGalleryResponse);
            TestOutputHelper.WriteLine($"Response text: {aliceGalleryResponse.Response.Text}");
        }

        [Fact]
        public void InvalidObject()
        {
            SportsRequest sportsRequest = null;
            using var scope = _services.CreateScope();
            var aliceService = scope.ServiceProvider.GetService<IAliceService>();
            var exception = Assert.Throws<ArgumentNullException>(() => aliceService.ProcessRequest(sportsRequest));
            Assert.Equal(nameof(sportsRequest), exception.ParamName);
        }
    }
}
