using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sports.SportsRu.Api.Models
{
    public class HotContentResponse
    {
        [JsonPropertyName("news")]
        public int[] News { get; set; }
    }
}
