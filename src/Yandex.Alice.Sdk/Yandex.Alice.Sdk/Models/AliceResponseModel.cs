using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace Yandex.Alice.Sdk.Models
{
    public class AliceResponseModel
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("tts")]
        public string Tts { get; set; }

        [JsonPropertyName("end_session")]
        public bool EndSession { get; set; }

        [JsonPropertyName("buttons")]
        public List<AliceButtonModel> Buttons { get; set; }
    }
}
