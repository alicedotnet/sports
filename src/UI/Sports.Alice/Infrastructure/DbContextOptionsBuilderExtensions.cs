using Microsoft.EntityFrameworkCore;
using Sports.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Alice.Infrastructure
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static void ConfigureSportsContextOptions(this DbContextOptionsBuilder builder
            , string connectionString)
        {
            string assemblyName = typeof(DbContextOptionsBuilderExtensions).Assembly.GetName().Name;
            builder.UseLazyLoadingProxies()
                .UseSqlite(connectionString, b => b.MigrationsAssembly(assemblyName));
        }
    }
}
