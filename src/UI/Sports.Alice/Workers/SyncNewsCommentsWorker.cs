using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sports.Alice.Workers
{
    public class SyncNewsCommentsWorker : Worker
    {
        protected override TimeSpan TimerInterval { get; }

        public SyncNewsCommentsWorker(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            TimerInterval = TimeSpan.FromMinutes(10);
        }

        protected override async void DoWork(object state)
        {
            using var scope = ServiceProvider.CreateScope();
            try
            {                
                var syncService = scope.ServiceProvider.GetService<ISyncService>();
                await syncService
                    .SyncPopularNewsCommentsAsync(DateTimeOffset.UtcNow.AddDays(-1), 10).ConfigureAwait(false);
            }
            catch(Exception e)
            {
                var logger = scope.ServiceProvider.GetService<ILogger<SyncNewsCommentsWorker>>();
                logger.LogError(e, string.Empty);
            }
        }
    }
}
