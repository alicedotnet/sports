using Sports.Alice.Models;
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
            var aliceRequest = new AliceRequest()
            {
                Session = new AliceSessionModel(),
                Request = new AliceRequestModel()
                {
                    Type = AliceRequestType.ButtonPressed,
                    Payload = new AliceCommand(AliceCommandType.BestComments)
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
        public async Task TestLatestNewsPayload()
        {
            var aliceRequest = new AliceRequest()
            {
                Session = new AliceSessionModel(),
                Request = new AliceRequestModel()
                {
                    Type = AliceRequestType.ButtonPressed,
                    Payload = new AliceCommand(AliceCommandType.LatestNews)
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
