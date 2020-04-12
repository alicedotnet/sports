using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Sports.SportsRu.Api.Helpers
{
    static class HttpHelper
    {
        public static string UrlEncodeJson<T>(T value)
        {
            string json = JsonSerializer.Serialize(value);
            return WebUtility.UrlEncode(json);
        }
    }
}
