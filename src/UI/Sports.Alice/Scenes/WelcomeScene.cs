using Sports.Alice.Models;
using Sports.Alice.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Scenes
{
    public class WelcomeScene : Scene
    {
        public override SceneType SceneType => SceneType.Welcome;

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
            var buttons = new List<AliceButtonModel>()
                {
                    new SportsButtonModel(Sports_Resources.Command_LatestNews),
                    new SportsButtonModel(Sports_Resources.Command_MainNews),
                    new SportsButtonModel(Sports_Resources.Command_BestComments)
                };
            return new SportsResponse(sportsRequest, Sports_Resources.Welcome_Text, buttons);
        }
    }
}
