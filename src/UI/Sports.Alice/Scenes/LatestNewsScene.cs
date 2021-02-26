using Sports.Alice.Models;
using Sports.Alice.Models.Settings;
using Sports.Alice.Resources;
using Sports.Data.Entities;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Alice.Sdk.Helpers;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Scenes
{
    public class LatestNewsScene : NewsScene
    {
        public override SceneType SceneType => SceneType.LatestNews;

        private readonly INewsService _newsService;
        private readonly SportsSettings _sportsSettings;

        public LatestNewsScene(INewsService newsService, SportsSettings sportsSettings)
        {
            _newsService = newsService;
            _sportsSettings = sportsSettings;
        }


        public override IAliceResponseBase Reply(SportsRequest sportsRequest)
        {
            var buttons = new List<AliceButtonModel>()
                {
                    new SportsButtonModel(Sports_Resources.Command_MainNews),
                    new SportsButtonModel(Sports_Resources.Command_BestComments)
                };
            SportKind sportKind = GetSportKind(sportsRequest);
            var news = _newsService.GetLatestNews(_sportsSettings.NewsToDisplay, sportKind);
            if (news.Any())
            {
                var titles = news.Select(x => x.Title);
                string headerTts = GetSportKindText(Sports_Resources.LatestNews_Header_Tts, sportKind);
                string text = $"{headerTts}\n\n{string.Join("\n\n", titles)}";
                string tts = $"{headerTts}{AliceHelper.SilenceString500}{string.Join(AliceHelper.SilenceString500, titles)}{AliceHelper.SilenceString1000}{Sports_Resources.Tips_Help}";
                var response = new SportsGalleryResponse(sportsRequest, text, tts, buttons);
                response.Response.Card = new AliceGalleryCardModel
                {
                    Items = new List<AliceGalleryCardItem>(),
                    Header = new AliceGalleryCardHeaderModel(GetSportKindText(Sports_Resources.LatestNews_Header, sportKind))
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
                response.Response.Buttons.AddRange(new List<AliceButtonModel>()
                {
                    new AliceButtonModel("футбол"),
                    new AliceButtonModel("хоккей"),
                    new AliceButtonModel("баскетбол"),
                    new AliceButtonModel("все")
                });
                response.SessionState.SportKind = sportKind;
                response.SessionState.CurrentScene = SceneType;
                return response;
            }
            else
            {
                var response = new SportsResponse(sportsRequest, Sports_Resources.LatestNews_NoNews, buttons);
                response.SessionState.CurrentScene = SceneType;
                return response;
            }
        }
    }
}
