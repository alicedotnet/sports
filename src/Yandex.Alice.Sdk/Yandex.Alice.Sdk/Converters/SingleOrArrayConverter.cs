using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Yandex.Alice.Sdk.Converters
{
    public abstract class SingleOrArrayConverter<TItem> : SingleOrArrayConverter<List<TItem>, TItem>
    {
        public SingleOrArrayConverter() : this(true) { }
        public SingleOrArrayConverter(bool canWrite) : base(canWrite) { }
    }

    public abstract class SingleOrArrayConverter<TCollection, TItem> : JsonConverter<TCollection>
        where TCollection : class, ICollection<TItem>, new()
    {
        public SingleOrArrayConverter() : this(true) { }
        public SingleOrArrayConverter(bool canWrite) => CanWrite = canWrite;

        public bool CanWrite { get; }

        public override TCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.StartArray:
                    var list = new TCollection();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                            break;
                        list.Add(ToItem(ref reader, options));
                    }
                    return list;
                default:
                    var item = ToItem(ref reader, options);
                    return new TCollection { item };
            }
        }

        protected abstract TItem ToItem(ref Utf8JsonReader reader, JsonSerializerOptions options);

        public override void Write(Utf8JsonWriter writer, TCollection value, JsonSerializerOptions options)
        {
            if (CanWrite && value.Count == 1)
            {
                JsonSerializer.Serialize(writer, value.First(), options);
            }
            else
            {
                writer.WriteStartArray();
                foreach (var item in value)
                    JsonSerializer.Serialize(writer, item, options);
                writer.WriteEndArray();
            }
        }
    }
}
