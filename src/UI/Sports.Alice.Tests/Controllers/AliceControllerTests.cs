using Sports.Alice.Models;
using Sports.Alice.Scenes;
using Sports.Alice.Tests.TestsInfrastructure;
using Sports.Alice.Tests.TestsInfrastructure.Fixtures;
using Sports.Common.Tests;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Tests.Controllers
{
    [Collection(TestsConstants.ServerCollectionName)]
    public class AliceControllerTests : BaseTests
    {
        private readonly HttpClient _httpClient;

        public AliceControllerTests(ServerFixture serverFixture, ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            _httpClient = serverFixture.HttpClient;
        }

        [Fact]
        public async Task TestWebhook()
        {
            string requestData = File.ReadAllText(TestsConstants.Assets.AliceRequestFilePath);
            using var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri("alice", UriKind.Relative), requestContent).ConfigureAwait(false);
            Assert.True(response.IsSuccessStatusCode, response.ToString());
            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            TestOutputHelper.WriteLine(responseContent);
        }

        [Fact]
        public async Task TestBestCommentsPayload()
        {
            var aliceRequest = new SportsRequest()
            {
                Session = new AliceSessionModel(),
                Request = new AliceRequestModel<SportsIntents>()
                {
                    Type = AliceRequestType.ButtonPressed,
                    Nlu = new AliceNluModel<SportsIntents>()
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
            string json = JsonSerializer.Serialize(aliceRequest);
            using var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri("alice", UriKind.Relative), requestContent).ConfigureAwait(false);
            Assert.True(response.IsSuccessStatusCode, response.ToString());
            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            TestOutputHelper.WriteLine(responseContent);
        }

        [Fact]
        public async Task TestLatestNews()
        {
            var aliceRequest = new SportsRequest()
            {
                Session = new AliceSessionModel(),
                Request = new AliceRequestModel<SportsIntents>()
                {
                    Nlu = new AliceNluModel<SportsIntents>()
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
            string json = JsonSerializer.Serialize(aliceRequest);
            using var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri("alice", UriKind.Relative), requestContent).ConfigureAwait(false);
            Assert.True(response.IsSuccessStatusCode, response.ToString());
            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            TestOutputHelper.WriteLine(responseContent);
        }
    }
}
