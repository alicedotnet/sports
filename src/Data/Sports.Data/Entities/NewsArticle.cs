using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Data.Entities
{
    public class NewsArticle
    {
        public Guid NewsArticleId { get; set; }
        public string Title { get; set; }
        public string ExternalId { get; set; }
    }
}
