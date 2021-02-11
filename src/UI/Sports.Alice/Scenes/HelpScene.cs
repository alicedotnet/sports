using Sports.Alice.Models;
using Sports.Alice.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Scenes
{
    public class HelpScene : Scene
    {
        public override SceneType SceneType => SceneType.BestComments;

        public override Scene MoveToNextScene(SportsRequest sportsRequest)
        {
            return null;
        }

        public override IAliceResponseBase Repeat(SportsRequest sportsRequest)
        {
            return Reply(sportsRequest);
        }

        public override IAliceResponseBase Reply(SportsRequest sportsRequest)
        {
            return Fallback(sportsRequest);
        }
    }
}
