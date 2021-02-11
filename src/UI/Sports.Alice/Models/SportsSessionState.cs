using Sports.Alice.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sports.Alice.Models
{
    public class SportsSessionState
    {
        [JsonPropertyName("nextNewsArticleId")]
        public Guid NextNewsArticleId { get; set; }

        [JsonPropertyName("currentScene")]
        public SceneType CurrentScene { get; set; }
    }
}
