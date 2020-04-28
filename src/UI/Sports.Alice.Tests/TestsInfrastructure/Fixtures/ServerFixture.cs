using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sports.Alice.Workers;
using Sports.Common.Tests.Extensions;
using Sports.Data.Context;
using Sports.Data.Entities;
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
            InitializeDatabase(Services);
            TestServer testServer = host.GetTestServer();
            HttpClient = testServer.CreateClient();
        }

        private void SetupMockContext(HostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            services.RemoveWorker<SyncNewsWorker>();
            services.RemoveWorker<SyncNewsCommentsWorker>();
            services.RemoveWorker<CleanWorker>();

            services.AddDbContext<SportsContext>(builder => builder.UseInMemoryDatabase("sports"));
        }

        private void InitializeDatabase(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetService<SportsContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            if(!context.NewsArticles.Any())
            {
                for (int i = 0; i < 6; i++)
                {
                    context.NewsArticles.Add(new NewsArticle() { Title = "test", PublishedDate = DateTime.Now });
                }
                context.SaveChanges();
            }
        }
    }
}
