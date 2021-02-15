using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sports.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Alice.Infrastructure
{
    public class SportsContextDesignTimeFactory : IDesignTimeDbContextFactory<SportsContext>
    {
        public SportsContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SportsContext>();
            builder.ConfigureSportsContextOptions("Data Source=sports.db");
            return new SportsContext(builder.Options);
        }
    }
}
