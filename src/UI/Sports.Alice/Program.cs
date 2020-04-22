using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sports.Alice
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging((ctx, logging) => {
                    //workaround for serilog
                    Environment.CurrentDirectory = ctx.HostingEnvironment.ContentRootPath;
                    var configuration = ctx.Configuration.GetSection("Logging");
                    logging.AddConfiguration(configuration);
                    logging.AddFile(configuration);
                    logging.AddConsole();
                });
    }
}
