using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Yandex.Alice.Sdk.Models
{
    public class AliceResponse
    {
        [JsonPropertyName("response")]
        public AliceResponseModel Response { get; set; }

        [JsonPropertyName("session")]
        public AliceSessionModel Session { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; } = "1.0";
    }
}
