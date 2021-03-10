using Sports.Alice.Models;
using Sports.Alice.Models.Settings;
using Sports.Alice.Resources;
using Sports.Data.Entities;
using Sports.Data.Models;
using Sports.Models;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Alice.Sdk.Helpers;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Scenes
{
    public abstract class NewsScene : Scene
    {
        protected abstract SportsButtonModel[] Buttons { get; }
        protected abstract string HeaderTts { get; }
        protected abstract string HeaderText { get; }
        protected abstract string NoNewsText { get; }
        protected INewsService NewsService { get; }
        protected int NewsPerPage { get; }

        public NewsScene(INewsService newsService, SportsSettings sportsSettings)
        {
            NewsService = newsService;
            NewsPerPage = sportsSettings.NewsToDisplay;
        }

        protected abstract PagedResponse<NewsArticleModel> GetNews(int pageIndex, SportKind sportKind);

        public override Scene MoveToNextScene(SportsRequest sportsRequest)
        {
            if (sportsRequest.Request.Nlu.Intents != null)
            {
                if(sportsRequest.Request.Nlu.Intents.Read != null
                    && sportsRequest.Request.Nlu.Intents.Read.Slots.Sport != null
                    && sportsRequest.Request.Nlu.Intents.Read.Slots.Sport.Value != SportKind.Undefined)
                {
                    return this;
                }
                if(sportsRequest.Request.Nlu.Intents.IsNext
                    || sportsRequest.Request.Nlu.Intents.IsBack)
                {
                    return this;
                }
            }
            return null;
        }

        public override IAliceResponseBase Repeat(SportsRequest sportsRequest)
        {
            return Reply(sportsRequest);
        }

        public override IAliceResponseBase Reply(SportsRequest sportsRequest)
        {
            SportKind sportKind = GetSportKind(sportsRequest);
            int pageIndex = GetPageIndex(sportsRequest);
            var newsResponse = GetNews(pageIndex, sportKind);
            int maxPageIndex = (int)Math.Ceiling(newsResponse.Total / (float)NewsPerPage) - 1;

            var buttons = new List<AliceButtonModel>();
            if (pageIndex > 0)
            {
                buttons.Add(new SportsButtonModel("назад"));
            }
            if(pageIndex >= 0  && pageIndex < maxPageIndex)
            {
                buttons.Add(new SportsButtonModel("вперед"));
            }
            buttons.AddRange(Buttons);
            
            var news = newsResponse.Items;
            if (news.Any())
            {
                var titles = news.Select(x => x.Title);
                string headerTts = GetSportKindText(HeaderTts, sportKind);
                string text = $"{headerTts}\n\n{string.Join("\n\n", titles)}";
                string tts = $"{headerTts}{AliceHelper.SilenceString500}{string.Join(AliceHelper.SilenceString500, titles)}{AliceHelper.SilenceString1000}{Sports_Resources.Tips_Help}";
                var response = new SportsGalleryResponse(sportsRequest, text, tts, buttons);
                response.Response.Card = new AliceGalleryCardModel
                {
                    Items = new List<AliceGalleryCardItem>(),
                    Header = new AliceGalleryCardHeaderModel(GetSportKindText(HeaderText, sportKind))
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
                response.SessionState.PageIndex = pageIndex;
                response.SessionState.SportKind = sportKind;
                response.SessionState.CurrentScene = SceneType;
                return response;
            }
            else
            {
                var response = new SportsResponse(sportsRequest, NoNewsText, buttons);
                response.SessionState.CurrentScene = SceneType;
                return response;
            }
        }

        private int GetPageIndex(SportsRequest sportsRequest)
        {
            int pageIndex = 0;
            if(sportsRequest.State.Session.CurrentScene == SceneType)
            {
                pageIndex = sportsRequest.State.Session.PageIndex;
                if (sportsRequest.Request.Nlu.Intents != null)
                {
                    if (sportsRequest.Request.Nlu.Intents.IsNext)
                    {
                        pageIndex++;
                    }
                    else if (sportsRequest.Request.Nlu.Intents.IsBack)
                    {
                        pageIndex--;
                    }
                }
            }
            return pageIndex;
        }

        private SportKind GetSportKind(SportsRequest sportsRequest)
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
