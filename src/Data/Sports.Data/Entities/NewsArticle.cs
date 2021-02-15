using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Data.Entities
{
    public class NewsArticle
    {
        public Guid NewsArticleId { get; set; }
        public string Title { get; set; }
        public DateTime? PublishedDate { get; set; }
        public Uri Url { get; set; }
        public string ExternalId { get; set; }
        public bool IsHotContent { get; set; }
        public int CommentsCount { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<NewsArticleComment> Comments { get; set; }
    }
}
