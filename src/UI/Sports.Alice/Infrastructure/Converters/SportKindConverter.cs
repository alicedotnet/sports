using Sports.Alice.Models;
using Sports.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sports.Alice.Infrastructure.Converters
{
    public class SportKindConverter : JsonConverter<SportKind>
    {
        public override SportKind Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string sport = reader.GetString();
            return sport switch
            {
                "football" => SportKind.Football,
                "hockey" => SportKind.Hockey,
                "basketball" => SportKind.Basketball,
                "all" => SportKind.All,
                _ => throw new Exception($"Unknown Sport: {sport}"),
            };
        }

        public override void Write(Utf8JsonWriter writer, SportKind value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToLower());
        }
    }
}
