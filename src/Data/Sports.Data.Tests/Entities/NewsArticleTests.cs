using Microsoft.EntityFrameworkCore;
using Sports.Data.Context;
using Sports.Data.Entities;
using System;
using Xunit;

namespace Sports.Data.Tests.Entities
{
    public class NewsArticleTests
    {
        [Fact]
        public void TestCreation()
        {
            var builder = new DbContextOptionsBuilder();
            builder
                .UseLazyLoadingProxies()
                .UseSqlite("Data Source=sports.db");
            using var db = new SportsContext(builder.Options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            var article = new NewsArticle() { Title = "test" };
            db.NewsArticles.Add(article);
            db.SaveChanges();
            Assert.NotEqual(Guid.Empty, article.NewsArticleId);
        }
    }
}
