using Microsoft.EntityFrameworkCore;
using Sports.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Data.Context
{
    public class SportsContext : DbContext
    {
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<NewsArticleComment> NewsArticlesComments { get; set; }

        public SportsContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsArticleComment>()
                .HasOne(nac => nac.NewsArticle)
                .WithMany(na => na.Comments)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
