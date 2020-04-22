using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Yandex.Alice.Sdk.Models
{
    public abstract class AliceResponseBase<T>
    {
        [JsonPropertyName("response")]
        public T Response { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}
