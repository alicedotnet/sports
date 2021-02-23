using Sports.Alice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sports.Alice.Infrastructure.Converters
{
    public class InfoTypeConverter : JsonConverter<InfoType>
    {
        public override InfoType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string infoTypeString = reader.GetString();
            return infoTypeString switch
            {
                "news" => InfoType.News,
                "comments" => InfoType.Comments,
                _ => throw new Exception($"Unknown InfoType: {infoTypeString}"),
            };
        }

        public override void Write(Utf8JsonWriter writer, InfoType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToLower());
        }
    }
}
