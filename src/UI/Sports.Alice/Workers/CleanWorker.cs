using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sports.Alice.Models.Settings;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Alice.Workers
{
    public class CleanWorker : Worker
    {
        protected override TimeSpan TimerInterval { get; }

        private readonly int _daysToKeepData;

        public CleanWorker(IOptions<SportsSettings> sportsSettings, IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            if(sportsSettings == null)
            {
                throw new ArgumentNullException(nameof(sportsSettings));
            }
            TimerInterval = TimeSpan.FromDays(1);
            _daysToKeepData = sportsSettings.Value.DaysToKeepData;
        }

        protected override void DoWork(object state)
        {
            using var scope = ServiceProvider.CreateScope();
            try
            {
                var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                var date = DateTimeOffset.Now.AddDays(-_daysToKeepData);
                syncService.DeleteOldData(date);
            }
            catch (Exception e)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<CleanWorker>>();
                logger.LogError(e, string.Empty);
            }
        }
    }
}
