using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Yandex.Alice.Sdk.Models;

namespace Yandex.Alice.Sdk.Converters
{
    public class AliceRequestTypeConverter : JsonConverter<AliceRequestType>
    {
        public override AliceRequestType Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            string input = reader.GetString();
            switch(input)
            {
                case "SimpleUtterance":
                    return AliceRequestType.SimpleUtterance;
                case "ButtonPressed":
                    return AliceRequestType.ButtonPressed;
                default:
                    throw new Exception("Unknown request type");
            }
        }

        public override void Write(
            Utf8JsonWriter writer,
            AliceRequestType requestType,
            JsonSerializerOptions options)
        {
            string result = requestType.ToString();
            writer.WriteStringValue(result);
        }
    }
}
