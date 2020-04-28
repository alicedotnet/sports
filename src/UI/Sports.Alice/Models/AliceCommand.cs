using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sports.Alice.Models
{
    public class AliceCommand
    {
        [JsonPropertyName("type")]
        public AliceCommandType Type { get; set; }
        [JsonPropertyName("payload")]
        public object Payload { get; set; }

        public AliceCommand()
        {

        }

        public AliceCommand(AliceCommandType type, object payload = null)
        {
            Type = type;
            Payload = payload;
        }
    }
}
