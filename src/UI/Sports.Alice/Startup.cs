using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sports.Alice.Models.Settings;
using Sports.Alice.Services;
using Sports.Alice.Services.Interfaces;
using Sports.Alice.Workers;
using Sports.Data.Context;
using Sports.Services;
using Sports.Services.Interfaces;
using Sports.SportsRu.Api.Services;
using Sports.SportsRu.Api.Services.Interfaces;

namespace Sports.Alice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IAliceService, AliceService>();
            services.AddScoped<ISyncService, SyncService>();
            services.AddScoped<ISportsRuApiService, SportsRuApiService>();
            services.AddScoped<INewsService, NewsService>();

            services.Configure<SportsSettings>(Configuration.GetSection("SportsSettings"));

            string connectionString = Configuration.GetConnectionString("database");
            string assemblyName = this.GetType().Assembly.GetName().Name;
            services.AddDbContext<SportsContext>(builder => builder
                .UseSqlite(connectionString, b => b.MigrationsAssembly(assemblyName)));

            services.AddHostedService<SyncWorker>();
            services.AddHostedService<CleanWorker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, SportsContext sportsContext)
        {
            sportsContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
