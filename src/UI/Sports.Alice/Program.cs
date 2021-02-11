using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sports.Data.Context;

namespace Sports.Alice
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .ConfigureServices(ConfigureContext)
                .Build();
            InitializeContext(host.Services);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConfigureContext(HostBuilderContext ctx, IServiceCollection services)
        {
            string connectionString = ctx.Configuration.GetConnectionString("database");
            string assemblyName = typeof(Program).Assembly.GetName().Name;
            services.AddDbContext<SportsContext>(builder => builder
                .UseLazyLoadingProxies()
                .UseSqlite(connectionString, b => b.MigrationsAssembly(assemblyName)));
        }

        private static void InitializeContext(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var sportsContext = scope.ServiceProvider.GetService<SportsContext>();
            sportsContext.Database.Migrate();
        }
    }
}
