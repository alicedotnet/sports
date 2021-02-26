using Sports.Alice.Models;
using Sports.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Scenes
{
    public abstract class NewsScene : Scene
    {
        public override Scene MoveToNextScene(SportsRequest sportsRequest)
        {
            if (sportsRequest.Request.Nlu.Intents != null
                && sportsRequest.Request.Nlu.Intents.Read != null
                && sportsRequest.Request.Nlu.Intents.Read.Slots.Sport != null
                && sportsRequest.Request.Nlu.Intents.Read.Slots.Sport.Value != SportKind.Undefined)
            {
                return this;
            }
            return null;
        }

        public override IAliceResponseBase Repeat(SportsRequest sportsRequest)
        {
            return Reply(sportsRequest);
        }

        protected SportKind GetSportKind(SportsRequest sportsRequest)
        {
            SportKind sportKind = SportKind.Undefined;
            if(sportsRequest.Request.Nlu.Intents != null
                && sportsRequest.Request.Nlu.Intents.Read != null
                && sportsRequest.Request.Nlu.Intents.Read.Slots.Sport != null
                && sportsRequest.Request.Nlu.Intents.Read.Slots.Sport.Value != SportKind.Undefined)
            {
                sportKind = sportsRequest.Request.Nlu.Intents.Read.Slots.Sport.Value;
            }
            else if(sportsRequest.State.Session != null
                && sportsRequest.State.Session.CurrentScene == SceneType
                && sportsRequest.State.Session.SportKind != SportKind.Undefined)
            {
                sportKind = sportsRequest.State.Session.SportKind;
            }
            return sportKind;
        }

        protected string GetSportKindText(string value, SportKind sportKind)
        {
            string sportKindText = sportKind switch
            {
                SportKind.Football => "футбола",
                SportKind.Basketball => "баскетбола",
                SportKind.Hockey => "хоккея",
                _ => string.Empty,
            };
            return string.Join(' ', value, sportKindText);
        }
    }
}
