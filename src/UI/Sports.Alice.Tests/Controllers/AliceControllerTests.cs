using Sports.Alice.Tests.TestsInfrastructure;
using Sports.Alice.Tests.TestsInfrastructure.Fixtures;
using Sports.Common.Tests;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

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
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("alice", requestContent);
            Assert.True(response.IsSuccessStatusCode, response.ToString());
            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            TestOutputHelper.WriteLine(responseContent);
        }
    }
}
