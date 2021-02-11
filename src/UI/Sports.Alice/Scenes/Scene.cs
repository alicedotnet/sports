using Sports.Alice.Models;
using Sports.Alice.Resources;
using Sports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Scenes
{
    public abstract class Scene
    {
        public abstract SceneType SceneType { get; }

        public abstract Scene MoveToNextScene(SportsRequest sportsRequest);
        public abstract IAliceResponseBase Reply(SportsRequest sportsRequest);
        public abstract IAliceResponseBase Repeat(SportsRequest sportsRequest);
        public virtual IAliceResponseBase Fallback(SportsRequest sportsRequest)
        {
            var buttons = new List<AliceButtonModel>()
                {
                    new SportsButtonModel(Sports_Resources.Command_LatestNews),
                    new SportsButtonModel(Sports_Resources.Command_MainNews),
                    new SportsButtonModel(Sports_Resources.Command_BestComments)
                };
            return new SportsResponse(sportsRequest, Sports_Resources.Help_Text_Tts, buttons);
        }

        protected static string GetTitleEnding(NewsArticleModel newsArticle)
        {
            return newsArticle.IsHotContent ? $" {EmojiLibrary.FireEmoji} {newsArticle.CommentsCount}"
                : newsArticle.CommentsCount > 0 ? $" {EmojiLibrary.SpeechBalloon} {newsArticle.CommentsCount} "
                    : string.Empty;
        }
    }
}
