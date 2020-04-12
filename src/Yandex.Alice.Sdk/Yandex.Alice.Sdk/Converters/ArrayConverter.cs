using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Yandex.Alice.Sdk.Converters
{
    public abstract class ArrayConverter<TItem> : JsonConverter<TItem[]>
    {
        public ArrayConverter() : this(true) { }
        public ArrayConverter(bool canWrite) => CanWrite = canWrite;

        public bool CanWrite { get; }

        public override TItem[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.StartArray:
                    var list = new List<TItem>();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                            break;
                        list.Add(ToItem(ref reader, options));
                    }
                    return list.ToArray();
                default:
                    return null;
            }
        }

        protected abstract TItem ToItem(ref Utf8JsonReader reader, JsonSerializerOptions options);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public override void Write(Utf8JsonWriter writer, TItem[] value, JsonSerializerOptions options)
        {
            if(value != null && CanWrite)
            {
                writer.WriteStartArray();
                foreach (var item in value)
                {
                    JsonSerializer.Serialize(writer, item, options);
                }
                writer.WriteEndArray();
            }
        }
    }
}
