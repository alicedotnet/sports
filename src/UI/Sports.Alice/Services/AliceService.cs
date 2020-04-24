using Microsoft.Extensions.Options;
using Sports.Alice.Models.Settings;
using Sports.Alice.Services.Interfaces;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Services
{
    public class AliceService : IAliceService
    {
        private readonly INewsService _newsService;
        private readonly SportsSettings _sportsSettings;

        public AliceService(INewsService newsService, IOptions<SportsSettings> sportsSettings)
        {
            if(sportsSettings == null)
            {
                throw new ArgumentNullException(nameof(sportsSettings));
            }

            _newsService = newsService;
            _sportsSettings = sportsSettings.Value;
        }

        public AliceResponseBase ProcessRequest(AliceRequest aliceRequest)
        {
            if(aliceRequest == null)
            {
                throw new ArgumentNullException(nameof(aliceRequest));
            }

            var buttons = new List<AliceButtonModel>()
            {
                new AliceButtonModel()
                {
                    Title = "последние новости"
                }
            };
            if (aliceRequest.Session.New || 
                aliceRequest.Request.Nlu.Tokens.Contains("новости") ||
                aliceRequest.Request.Nlu.Tokens.Contains("расскажи"))
            {
                var news = _newsService.GetLatestNews(_sportsSettings.NewsToDisplay);
                if (news.Any())
                {
                    var titles = news.Select(x => x.Title);
                    string text = "Вот последние новости\n\n" + string.Join("\n\n", titles);
                    string tts = "Вот последние новости. " + string.Join(". ", titles);
                    var response = new AliceGalleryResponse(aliceRequest, text, tts, buttons);
                    response.Response.Card.Items = new List<AliceGalleryCardItem>();
                    response.Response.Card.Header = new AliceGalleryCardHeaderModel("Последние новости");
                    foreach (var newsArticle in news)
                    {
                        response.Response.Card.Items.Add(new AliceGalleryCardItem()
                        {
                            Title = newsArticle.Title,
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
                    return new AliceResponse(aliceRequest, "У меня нет новостей", buttons);
                }
            }
            else
            {
                return new AliceResponse(aliceRequest, "Вы можете попросить меня прочитать последние новости спорта сказав фразу: расскажи новости", buttons);
            }
        }
    }
}
