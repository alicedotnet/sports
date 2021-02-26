using Sports.Alice.Infrastructure.Converters;
using Sports.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Yandex.Alice.Sdk;
using Yandex.Alice.Sdk.Helpers;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Models
{
    public class SportsIntents
    {
        [JsonPropertyName("read")]
        public AliceIntentModel<ReadSlots> Read { get; set; }

        [JsonPropertyName("next")]
        public AliceIntentModel Next { get; set; }

        [JsonPropertyName(AliceConstants.AliceIntents.Help)]
        public AliceIntentModel YandexHelp { get; set; }

        [JsonPropertyName(AliceConstants.AliceIntents.WhatCanYouDo)]
        public AliceIntentModel YandexWhatCanYouDo { get; set; }

        [JsonIgnore]
        public bool IsHelp
        {
            get
            {
                return YandexHelp != null || YandexWhatCanYouDo != null;
            }
        }

        [JsonPropertyName(AliceConstants.AliceIntents.Repeat)]
        public AliceIntentModel YandexRepeat { get; set; }

        [JsonPropertyName("repeat")]
        public AliceIntentModel Repeat { get; set; }

        public bool IsRepeat
        {
            get
            {
                return YandexRepeat != null || Repeat != null;
            }
        }

    }

    public class ReadSlots
    {
        [JsonPropertyName("infoCategory")]
        public AliceEntityInfoCategoryModel InfoCategory { get; set; }

        [JsonPropertyName("infoType")]
        public AliceEntityInfoTypeModel InfoType { get; set; }

        [JsonPropertyName("sport")]
        public AliceEntitySportModel Sport { get; set; }
    }

    public enum InfoCategory
    {
        Undefined,
        Main,
        Latest,
        Best
    }

    public class AliceEntityInfoCategoryModel : AliceEntityModel
    {
        [JsonPropertyName("value")]
        [JsonConverter(typeof(InfoCategoryConverter))]
        public InfoCategory Value { get; set; }
    }

    public enum InfoType
    {
        Undefined,
        News,
        Comments
    }

    public class AliceEntityInfoTypeModel : AliceEntityModel
    {
        [JsonPropertyName("value")]
        [JsonConverter(typeof(InfoTypeConverter))]
        public InfoType Value { get; set; }
    }

    public class AliceEntitySportModel : AliceEntityModel
    {
        [JsonPropertyName("value")]
        [JsonConverter(typeof(SportKindConverter))]
        public SportKind Value { get; set; }
    }
}
