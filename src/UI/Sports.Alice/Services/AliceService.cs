using Microsoft.Extensions.Logging;
using Sports.Alice.Models;
using Sports.Alice.Scenes;
using Sports.Alice.Services.Interfaces;
using System;
using System.Linq;
using System.Text.Json;
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

            if(sportsRequest.Request.Nlu.Intents.News != null)
            {
                if (sportsRequest.Request.Nlu.Intents.News.Slots.Main != null)
                {
                    currentSceneType = SceneType.MainNews;
                }
                else
                {
                    currentSceneType = SceneType.LatestNews;
                }
            }
            else if (sportsRequest.Request.Nlu.Intents.Comments != null)
            {
                currentSceneType = SceneType.BestComments;
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

            _fallbackLogger.LogInformation($"FALLBACK. Request: {0}", JsonSerializer.Serialize(sportsRequest));
            return currentScene.Fallback(sportsRequest);
        }
    }
}
