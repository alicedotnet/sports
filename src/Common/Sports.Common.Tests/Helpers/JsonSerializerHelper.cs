using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Sports.Common.Tests.Helpers
{
    static class JsonSerializerHelper
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static string ToPrettyJson<T>(T value)
        {
            return JsonSerializer.Serialize(value, _options);
        }
    }
}
