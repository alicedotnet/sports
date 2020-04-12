using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using SportsRu.Alice.Tests.TestsInfrastructure;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SportsRu.Alice.Tests.Controllers
{
    public class AliceControllerTests
    {
        private readonly HttpClient _httpClient;

        public AliceControllerTests()
        {
            var host = Program
                .CreateHostBuilder(Array.Empty<string>())
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseTestServer();
                })
                .Start();
            TestServer testServer = host.GetTestServer();
            _httpClient = testServer.CreateClient();
        }

        [Fact]
        public async Task TestWebhook()
        {
            string requestData = File.ReadAllText(TestsConstants.Assets.AliceRequestFilePath);
            var content = new StringContent(requestData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("alice", content);
            Assert.True(response.IsSuccessStatusCode, response.ToString());
        }
    }
}
