using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Buffers;

namespace Sports.Common.Extensions
{
    public static class JsonElementExtensions
    {
        public static T ToObject<T>(this JsonElement element)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
                element.WriteTo(writer);
            return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan);
        }
    }
}
