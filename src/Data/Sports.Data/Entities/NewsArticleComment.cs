using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Data.Entities
{
    public class NewsArticleComment
    {
        public Guid NewsArticleCommentId { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public Guid NewsArticleId { get; set; }
        public virtual NewsArticle NewsArticle { get; set; }
        public string ExternalId { get; set; }
    }
}
