using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Sports.Alice.Tests.TestsInfrastructure.Fixtures
{
    public class ServerFixture
    {
        public HttpClient HttpClient { get; }

        public ServerFixture()
        {
            var host = Program
                .CreateHostBuilder(Array.Empty<string>())
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseTestServer();
                })
                .Start();
            TestServer testServer = host.GetTestServer();
            HttpClient = testServer.CreateClient();
        }
    }
}
