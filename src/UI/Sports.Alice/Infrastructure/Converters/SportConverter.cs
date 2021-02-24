using Sports.Alice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sports.Alice.Infrastructure.Converters
{
    public class SportConverter : JsonConverter<Sport>
    {
        public override Sport Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string sport = reader.GetString();
            return sport switch
            {
                "football" => Sport.Football,
                "hockey" => Sport.Hockey,
                "basketball" => Sport.Basketball,
                _ => throw new Exception($"Unknown Sport: {sport}"),
            };
        }

        public override void Write(Utf8JsonWriter writer, Sport value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
