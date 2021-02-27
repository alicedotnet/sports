﻿using Sports.Data.Entities;
using Sports.Data.Models;
using Sports.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Services.Interfaces
{
    public interface INewsService
    {
        NewsArticleModel GetById(Guid id);
        IEnumerable<NewsArticleModel> GetLatestNews(PagedRequest pagedRequest, SportKind sportKind = SportKind.Undefined);
        IEnumerable<NewsArticleModel> GetPopularNews(DateTimeOffset fromDate, PagedRequest pagedRequest, SportKind sportKind = SportKind.Undefined);
        NewsArticleModel GetNextPopularNewsArticle(DateTimeOffset fromDate, Guid newsArticleId);
    }
}
