using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sports.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sports.Alice.Tests.TestsInfrastructure.Fixtures
{
    public class MockContextFixture
    {
        public IServiceProvider Services { get; }

        public MockContextFixture()
        {
            Services = Program
                .CreateHostBuilder(Array.Empty<string>())
                .ConfigureServices(SetupMockContext)
                .Build()
                .Services;
        }

        private void SetupMockContext(HostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            var descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(SportsContext));
            services.Remove(descriptor);

            services
                .AddDbContext<SportsContext>(builder => builder
                    .UseInMemoryDatabase("sports"));
        }
    }
}
