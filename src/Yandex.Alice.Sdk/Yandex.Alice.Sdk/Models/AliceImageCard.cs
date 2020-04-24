using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Yandex.Alice.Sdk.Converters;

namespace Yandex.Alice.Sdk.Models
{
    public class AliceImageCard
    {
        [JsonPropertyName("type")]
        [JsonConverter(typeof(AliceCardTypeConverter))]
        public AliceCardType Type { get; set; }
        [JsonPropertyName("image_id")]
        public string ImageId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("button")]
        public AliceImageCardButton Button { get; set; }
    }
}
