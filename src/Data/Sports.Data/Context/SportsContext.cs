using Microsoft.EntityFrameworkCore;
using Sports.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Data.Context
{
    public class SportsContext : DbContext
    {
        public SportsContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<NewsArticle> NewsArticles { get; set; }
    }
}
