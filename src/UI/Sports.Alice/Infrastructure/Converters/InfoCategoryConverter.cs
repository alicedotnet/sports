using Sports.Alice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sports.Alice.Infrastructure.Converters
{
    public class InfoCategoryConverter : JsonConverter<InfoCategory>
    {
        public override InfoCategory Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string infoCategoryString = reader.GetString();
            return infoCategoryString switch
            {
                "main" => InfoCategory.Main,
                "latest" => InfoCategory.Latest,
                "best" => InfoCategory.Best,
                _ => throw new Exception($"Unknown InfoCategory: {infoCategoryString}"),
            };
        }

        public override void Write(Utf8JsonWriter writer, InfoCategory value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToLower());
        }
    }
}
