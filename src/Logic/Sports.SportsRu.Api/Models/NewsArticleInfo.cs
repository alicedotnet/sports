using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sports.SportsRu.Api.Models
{
    public class NewsArticleInfo
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("body_is_empty")]
        public bool BodyIsEmpty { get; set; }
        [JsonPropertyName("published")]
        public NewsArticlePublished Published { get; set; }
    }

    public class NewsArticlePublished
    {
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
}
