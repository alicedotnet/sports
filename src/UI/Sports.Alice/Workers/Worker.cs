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
    public abstract class Worker : BackgroundService
    {
        protected abstract TimeSpan TimerInterval { get; }
        protected IServiceProvider ServiceProvider { get; }
        private Timer _timer;
        private bool _isFirstTimeRun;

        protected Worker(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            _isFirstTimeRun = true;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.Zero, TimerInterval);
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return base.StopAsync(cancellationToken);
        }

        protected void Execute(object state)
        {
            if(_isFirstTimeRun)
            {
                _isFirstTimeRun = false;
                return;
            }
            DoWork(state);
        }

        protected abstract void DoWork(object state);
    }
}
