using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sports.Alice.Workers;
using Sports.Common.Tests.Extensions;
using Sports.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Sports.Alice.Tests.TestsInfrastructure.Fixtures
{
    public class ServerFixture
    {
        public HttpClient HttpClient { get; }
        public IServiceProvider Services { get; }

        public ServerFixture()
        {
            var host = Program
                .CreateHostBuilder(Array.Empty<string>())
                .ConfigureServices(SetupMockContext)
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseTestServer();
                })
                .Start();
            Services = host.Services;
            TestServer testServer = host.GetTestServer();
            HttpClient = testServer.CreateClient();
        }

        private void SetupMockContext(HostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            services.Remove<SportsContext>();
            services.RemoveWorker<SyncNewsWorker>();
            services.RemoveWorker<SyncNewsCommentsWorker>();
            services.RemoveWorker<CleanWorker>();

            services
                .AddDbContext<SportsContext>(builder => builder
                    .UseInMemoryDatabase("sports"));
        }
    }
}
