using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sports.SportsRu.Api.Models
{
    class CommentIdsRequest
    {
        [JsonPropertyName("message_id")]
        public int MessageId { get; set; }
        [JsonPropertyName("message_class")]
        public string MessageClass { get; set; }
        [JsonPropertyName("sort")]
        public string Sort { get; set; }
    }
}
