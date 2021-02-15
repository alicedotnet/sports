using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sports.Alice.Infrastructure;
using Sports.Data.Context;
using Sports.Services.Interfaces;

namespace Sports.Alice
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .Build();
            await InitializeAsync(host.Services).ConfigureAwait(false);
            host.Run();
        }

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var sportsContext = scope.ServiceProvider.GetService<SportsContext>();
            sportsContext.Database.Migrate();

            var syncService = scope.ServiceProvider.GetService<ISyncService>();
            await syncService.SyncNewsAsync().ConfigureAwait(false);
            await syncService.SyncPopularNewsCommentsAsync().ConfigureAwait(false);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
