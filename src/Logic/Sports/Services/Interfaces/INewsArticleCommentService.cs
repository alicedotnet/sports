using Sports.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Services.Interfaces
{
    public interface INewsArticleCommentService
    {
        IEnumerable<NewsArticleCommentModel> GetBestComments(Guid newsArticleId, int commentsCount);
    }
}
