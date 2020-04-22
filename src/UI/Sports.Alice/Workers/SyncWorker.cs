using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sports.Alice.Workers
{
    public class SyncWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;
        private const int _secondsInterval = 60;

        public SyncWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_secondsInterval));
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return base.StopAsync(cancellationToken);
        }

        private async void DoWork(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
            await syncService.SyncAllAsync().ConfigureAwait(false);
        }
    }
}
