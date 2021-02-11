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
        [JsonPropertyName("news")]
        public AliceIntentModel<NewsSlots> News { get; set; }

        [JsonPropertyName("comments")]
        public AliceIntentModel Comments { get; set; }

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

    public class NewsSlots
    {
        [JsonPropertyName("main")]
        public AliceEntityStringModel Main { get; set; }

        [JsonPropertyName("latest")]
        public AliceEntityStringModel Latest { get; set; }
    }
}
