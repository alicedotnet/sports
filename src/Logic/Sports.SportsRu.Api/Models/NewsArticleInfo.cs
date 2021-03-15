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
        [JsonPropertyName("desktop_url")]
        public Uri DesktopUrl { get; set; }
        [JsonPropertyName("content_option")]
        public ContentOption ContentOption { get; set; }
        [JsonPropertyName("published")]
        public NewsArticlePublished Published { get; set; }
        [JsonPropertyName("comments_count")]
        public int CommentsCount { get; set; }
        [JsonPropertyName("section")]
        public ArticleSection Section { get; set; }
        [JsonPropertyName("main")]
        public bool? Main { get; set; }
    }

    public class ArticleSection
    {
        [JsonPropertyName("webname")]
        public string WebName { get; set; }
    }

    public class NewsArticlePublished
    {
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }

    public class ContentOption
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
