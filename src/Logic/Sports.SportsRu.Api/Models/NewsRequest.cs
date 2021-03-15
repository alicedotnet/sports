using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sports.SportsRu.Api.Models
{
    class NewsRequest
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("filter")]
        public NewsRequestFilter Filter { get; set; }
    }

    class NewsRequestFilter
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("news_type")]
        public string NewsPriority { get; set; }
        [JsonPropertyName("content_origin")]
        public string ContentOrigin { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
