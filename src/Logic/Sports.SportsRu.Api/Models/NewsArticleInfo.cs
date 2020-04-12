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
    }
}
