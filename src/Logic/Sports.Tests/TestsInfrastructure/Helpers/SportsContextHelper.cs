using Sports.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Tests.TestsInfrastructure.Helpers
{
    static class SportsContextHelper
    {
        public static void DeleteAllNews(SportsContext sportsContext)
        {
            sportsContext.NewsArticles.RemoveRange(sportsContext.NewsArticles);
            sportsContext.SaveChanges();
        }
    }
}
