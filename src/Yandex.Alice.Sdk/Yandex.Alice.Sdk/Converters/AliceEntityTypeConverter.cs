using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Yandex.Alice.Sdk.Models;

namespace Yandex.Alice.Sdk.Converters
{
    public class AliceEntityTypeConverter : JsonConverter<AliceEntityType>
    {
        public override AliceEntityType Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            string input = reader.GetString();
            switch (input)
            {
                case Constants.AliceEntityTypeValues.DateTime:
                    return AliceEntityType.YANDEX_DATETIME;
                case Constants.AliceEntityTypeValues.Fio:
                    return AliceEntityType.YANDEX_FIO;
                case Constants.AliceEntityTypeValues.Geo:
                    return AliceEntityType.YANDEX_GEO;
                case Constants.AliceEntityTypeValues.Number:
                    return AliceEntityType.YANDEX_NUMBER;
                default:
                    return AliceEntityType.Unknown;
            }
        }

        public override void Write(
            Utf8JsonWriter writer,
            AliceEntityType entityType,
            JsonSerializerOptions options)
        {
            string result;
            switch (entityType)
            {
                case AliceEntityType.YANDEX_DATETIME:
                    result = Constants.AliceEntityTypeValues.DateTime;
                    break;
                case AliceEntityType.YANDEX_FIO:
                    result = Constants.AliceEntityTypeValues.Fio;
                    break;
                case AliceEntityType.YANDEX_GEO:
                    result = Constants.AliceEntityTypeValues.Geo;
                    break;
                case AliceEntityType.YANDEX_NUMBER:
                    result = Constants.AliceEntityTypeValues.Number;
                    break;
                default:
                    result = "unknown";
                    break;
            };
            writer.WriteStringValue(result);
        }
    }
}
