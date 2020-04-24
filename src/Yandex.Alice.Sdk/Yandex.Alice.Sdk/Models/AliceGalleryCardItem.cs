using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Yandex.Alice.Sdk.Models
{
    public class AliceGalleryCardItem
    {
        [JsonPropertyName("image_id")]
        public string ImageId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("button")]
        public AliceImageCardButtonModel Button { get; set; }
    }
}
