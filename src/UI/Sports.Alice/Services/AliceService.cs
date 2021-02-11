using Microsoft.Extensions.Logging;
using Sports.Alice.Models;
using Sports.Alice.Scenes;
using Sports.Alice.Services.Interfaces;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Services
{
    public class AliceService : IAliceService
    {
        private readonly IScenesProvider _scenesProvider;
        private readonly ILogger _fallbackLogger;

        public AliceService(IScenesProvider scenesProvider, ILoggerFactory loggerFactory)
        {
            _scenesProvider = scenesProvider;
            _fallbackLogger = loggerFactory.CreateLogger("Fallback");
        }

        public IAliceResponseBase ProcessRequest(SportsRequest sportsRequest)
        {
            if (sportsRequest == null)
            {
                throw new ArgumentNullException(nameof(sportsRequest));
            }

            if(sportsRequest.Request.OriginalUtterance == "ping")
            {
                return new SportsResponse(sportsRequest, "Привет, Яндекс! У меня все в порядке :)");
            }

            SceneType currentSceneType = SceneType.Undefined;
            if (sportsRequest.Session.New)
            {
                currentSceneType = SceneType.LatestNews;
                sportsRequest.State.Session = new SportsSessionState();
            }

            if(sportsRequest.Request.Nlu.Intents.Read != null)
            {
                if (sportsRequest.Request.Nlu.Intents.Read.Slots.BestComments != null)
                {
                    currentSceneType = SceneType.BestComments;
                }
                else if (sportsRequest.Request.Nlu.Intents.Read.Slots.MainNews != null)
                {
                    currentSceneType = SceneType.MainNews;
                }
                else
                {
                    currentSceneType = SceneType.LatestNews;
                }
            }
            else if(sportsRequest.Request.Nlu.Intents.IsHelp)
            {
                currentSceneType = SceneType.Help;
            }

            Scene currentScene;
            if (currentSceneType != SceneType.Undefined)
            {
                sportsRequest.State.Session.CurrentScene = currentSceneType;
                currentScene = _scenesProvider.GetScene(currentSceneType);
                return currentScene.Reply(sportsRequest);
            }
            else
            {
                currentScene = _scenesProvider.GetScene(sportsRequest.State.Session.CurrentScene);
                if(sportsRequest.Request.Nlu.Intents.IsRepeat)
                {
                    return currentScene.Repeat(sportsRequest);
                }
                var nextScene = currentScene.MoveToNextScene(sportsRequest);
                if(nextScene != null)
                {
                    return nextScene.Reply(sportsRequest);
                }
            }

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            };
            var requestText = JsonSerializer.Serialize(sportsRequest, options);
            _fallbackLogger.LogInformation("FALLBACK. Request: {0}", requestText);
            return currentScene.Fallback(sportsRequest);
        }
    }
}
