using Microsoft.Extensions.DependencyInjection;
using Sports.Common.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sports.Common.Tests.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void Remove<T>(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var descriptor = services.First(x => x.ServiceType == typeof(T));
            services.Remove(descriptor);
        }

        public static void RemoveWorker<T>(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var descriptor = services.First(x => x.ImplementationType == typeof(T));
            services.Remove(descriptor);
        }
    }
}
