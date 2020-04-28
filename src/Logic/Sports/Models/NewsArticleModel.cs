using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Models
{
    public class NewsArticleModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Uri Url { get; set; }
        public int CommentsCount { get; set; }
        public bool IsHotContent { get; set; }
        public string ExternalId { get; set; }
    }
}
