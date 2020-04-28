using Sports.Data.Context;
using Sports.Models;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sports.Services
{
    public class NewsArticleCommentService : INewsArticleCommentService
    {
        private readonly SportsContext _sportsContext;

        public NewsArticleCommentService(SportsContext sportsContext)
        {
            _sportsContext = sportsContext;
        }

        public IEnumerable<NewsArticleCommentModel> GetBestComments(Guid newsArticleId, int commentsCount)
        {
            return _sportsContext
                .NewsArticlesComments
                .Where(x => x.NewsArticleId == newsArticleId)
                .OrderByDescending(x => x.Rating)
                .Take(commentsCount)
                .AsEnumerable()
                .GroupBy(x => x.NewsArticle)
                .FirstOrDefault()
                ?.Select(x => new NewsArticleCommentModel()
                {
                    CommentText = x.Text,
                    CommentRating = x.Rating
                });
        }
    }
}
