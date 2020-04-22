using Sports.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Services.Interfaces
{
    public interface INewsService
    {
        IEnumerable<NewsArticleModel> GetLatestNews(int newsCount);
    }
}
