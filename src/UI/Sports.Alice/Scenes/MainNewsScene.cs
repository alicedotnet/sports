using Sports.Alice.Models;
using Sports.Alice.Models.Settings;
using Sports.Alice.Resources;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Helpers;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Scenes
{
    public class MainNewsScene : Scene
    {
        public override SceneType SceneType => SceneType.BestComments;

        private readonly INewsService _newsService;
        private readonly SportsSettings _sportsSettings;

        public MainNewsScene(INewsService newsService, SportsSettings sportsSettings)
        {
            _newsService = newsService;
            _sportsSettings = sportsSettings;
        }

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
                    new SportsButtonModel(Sports_Resources.Command_BestComments)
                };
            var news = _newsService.GetPopularNews(DateTimeOffset.Now.AddDays(-1), _sportsSettings.NewsToDisplay);
            if (news.Any())
            {
                var titles = news.Select(x => x.Title);
                string text = $"{Sports_Resources.MainNews_Header_Tts}\n\n{string.Join("\n\n", titles)}";
                string tts = $"{Sports_Resources.MainNews_Header_Tts}{AliceHelper.SilenceString500}{string.Join(AliceHelper.SilenceString500, titles)}{AliceHelper.SilenceString1000}{Sports_Resources.Tips_Help}";
                var response = new SportsGalleryResponse(sportsRequest, text, tts, buttons);
                response.Response.Card = new AliceGalleryCardModel
                {
                    Items = new List<AliceGalleryCardItem>(),
                    Header = new AliceGalleryCardHeaderModel(Sports_Resources.MainNews_Header)
                };
                foreach (var newsArticle in news)
                {
                    response.Response.Card.Items.Add(new AliceGalleryCardItem()
                    {
                        Title = AliceHelper
                            .PrepareGalleryCardItemTitle(newsArticle.Title, GetTitleEnding(newsArticle), AliceHelper.DefaultReducedStringEnding),
                        Button = new AliceImageCardButtonModel()
                        {
                            Url = newsArticle.Url
                        }
                    });
                }
                return response;
            }
            else
            {
                return new SportsResponse(sportsRequest, Sports_Resources.MainNews_NoNews, buttons);
            }
        }
    }
}
