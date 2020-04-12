using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Yandex.Alice.Sdk.Converters;

namespace Yandex.Alice.Sdk.Models
{
    public class AliceNLUModel
    {
        [JsonPropertyName("tokens")]
        public string[] Tokens { get; set; }

        [JsonPropertyName("entities")]
        [JsonConverter(typeof(AliceEntityModelConverter))]
        public AliceEntityModel[] Entities { get; set; }
    }
}
