using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sports.SportsRu.Api.Models
{
    public class CommentByIdsResponse
    {
        [JsonPropertyName("data")]
        public CommentByIdsResponseData Data { get; set; }
    }

    public class CommentByIdsResponseData
    {
        [JsonPropertyName("comments")]
        public IEnumerable<CommentInfo> Comments { get; set; }
    }

    public class CommentInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("rating")]
        public CommentRating Rating { get; set; }
    }

    public class CommentRating
    {
        [JsonPropertyName("plus")]
        public int Plus { get; set; }
        [JsonPropertyName("minus")]
        public int Minus { get; set; }
    }

}
