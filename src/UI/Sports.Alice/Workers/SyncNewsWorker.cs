using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sports.Alice.Workers
{
    public class SyncNewsWorker : Worker
    {
        protected override TimeSpan TimerInterval { get; }

        public SyncNewsWorker(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            TimerInterval = TimeSpan.FromSeconds(60);
        }        

        protected override async void DoWork(object state)
        {
            using var scope = ServiceProvider.CreateScope();
            try
            {
                var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                await syncService.SyncNewsAsync().ConfigureAwait(false);
            }
            catch(Exception e)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<SyncNewsWorker>>();
                logger.LogError(e, string.Empty);
            }
        }
    }
}
