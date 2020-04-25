using Microsoft.Extensions.Options;
using Sports.Alice.Models;
using Sports.Alice.Models.Settings;
using Sports.Alice.Services.Interfaces;
using Sports.Models;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Yandex.Alice.Sdk.Helpers;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Services
{
    public class AliceService : IAliceService
    {
        private readonly INewsService _newsService;
        private readonly SportsSettings _sportsSettings;

        public AliceService(INewsService newsService, IOptions<SportsSettings> sportsSettings)
        {
            if (sportsSettings == null)
            {
                throw new ArgumentNullException(nameof(sportsSettings));
            }
            _newsService = newsService;
            _sportsSettings = sportsSettings.Value;
        }

        public AliceResponseBase ProcessRequest(AliceRequest aliceRequest)
        {
            if (aliceRequest == null)
            {
                throw new ArgumentNullException(nameof(aliceRequest));
            }

            AliceCommand aliceCommand = AliceCommand.Undefined;
            if (aliceRequest.Session.New)
            {
                aliceCommand = AliceCommand.LatestNews;
            }
            else if (aliceRequest.Request.Type == AliceRequestType.ButtonPressed)
            {
                if (aliceRequest.Request.Payload is JsonElement element && element.ValueKind == JsonValueKind.Number)
                {
                    aliceCommand = (AliceCommand)element.GetInt32();
                }
            }
            else if (aliceRequest.Request.Nlu.Tokens.Contains("новости"))
            {
                if (aliceRequest.Request.Nlu.Tokens.Contains("главные") || aliceRequest.Request.Nlu.Tokens.Contains("популярные"))
                {
                    aliceCommand = AliceCommand.MainNews;
                }
                else
                {
                    aliceCommand = AliceCommand.LatestNews;
                }
            }

            switch (aliceCommand)
            {
                case AliceCommand.LatestNews:
                    return GetLatestNews(aliceRequest);
                case AliceCommand.MainNews:
                    return GetMainNews(aliceRequest);
                default:
                    return GetHelp(aliceRequest);
            }
        }

        private AliceResponseBase GetLatestNews(AliceRequest aliceRequest)
        {
            var buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel()
                    {
                        Title = AliceCommands.MainNews,
                        Payload = AliceCommand.MainNews,
                        Hide = true
                    }
                };
            var news = _newsService.GetLatestNews(_sportsSettings.NewsToDisplay);
            if (news.Any())
            {
                var titles = news.Select(x => x.Title);
                string text = "Вот последние новости\n\n" + string.Join("\n\n", titles);
                string tts = "Вот последние новости. " + string.Join(". ", titles) + " sil <[1000]> Чтобы узнать дополнительные возможности скажите: помощь";
                var response = new AliceGalleryResponse(aliceRequest, text, tts, buttons);
                response.Response.Card.Items = new List<AliceGalleryCardItem>();
                response.Response.Card.Header = new AliceGalleryCardHeaderModel("Последние новости");
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
                return new AliceResponse(aliceRequest, "У меня нет последний новостей", buttons);
            }
        }

        private AliceResponseBase GetMainNews(AliceRequest aliceRequest)
        {
            var buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel()
                    {
                        Title = AliceCommands.LatestNews,
                        Payload = AliceCommand.LatestNews,
                        Hide = true
                    }
                };
            var news = _newsService.GetPopularNews(DateTimeOffset.Now.AddDays(-1), _sportsSettings.NewsToDisplay);
            if (news.Any())
            {
                var titles = news.Select(x => x.Title);
                string text = "Вот главные новости\n\n" + string.Join("\n\n", titles);
                string tts = "Вот главные новости. " + string.Join(". ", titles) + " sil <[1000]> Чтобы узнать дополнительные возможности скажите: помощь";
                var response = new AliceGalleryResponse(aliceRequest, text, tts, buttons);
                response.Response.Card.Items = new List<AliceGalleryCardItem>();
                response.Response.Card.Header = new AliceGalleryCardHeaderModel("Главные новости");
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
                return new AliceResponse(aliceRequest, "У меня нет главных новостей", buttons);
            }
        }

        private AliceResponseBase GetHelp(AliceRequest aliceRequest)
        {
            var buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel()
                    {
                        Title = AliceCommands.LatestNews,
                        Payload = AliceCommand.LatestNews
                    },
                    new AliceButtonModel()
                    {
                        Title = AliceCommands.MainNews,
                        Payload = AliceCommand.MainNews
                    }
                };
            string text = "Вы можете попросить меня прочитать последние новости спорта сказав фразу: последние новости или главные новости с помощью фразы: главные новости";
            string tts = "Вы можете попросить меня прочитать последние новости спорта сказав фразу: последние новости. или главные новости с помощью фразы: главные новости";
            return new AliceResponse(aliceRequest,text, tts, buttons);
        }

        private string GetTitleEnding(NewsArticleModel newsArticle)
        {
            return newsArticle.IsHotContent ? $" {EmojiLibrary.FireEmoji} {newsArticle.CommentsCount}"
                : newsArticle.CommentsCount > 0 ? $" {EmojiLibrary.SpeechBalloon} {newsArticle.CommentsCount} "
                    : string.Empty;
        }
    }
}
