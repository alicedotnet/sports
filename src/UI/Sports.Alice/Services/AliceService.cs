using Microsoft.Extensions.Options;
using Sports.Alice.Models.Settings;
using Sports.Alice.Services.Interfaces;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public AliceResponse ProcessRequest(AliceRequest aliceRequest)
        {
            if(aliceRequest == null)
            {
                throw new ArgumentNullException(nameof(aliceRequest));
            }

            string text;
            string tts;
            if (aliceRequest.Session.New || 
                aliceRequest.Request.Nlu.Tokens.Contains("новости") ||
                aliceRequest.Request.Nlu.Tokens.Contains("расскажи"))
            {
                var news = _newsService.GetLatestNews(_sportsSettings.NewsToDisplay);
                if (news.Any())
                {
                    var titles = news.Select(x => x.Title);
                    text = string.Join("\n\n", titles);
                    tts = string.Join(". ", titles);
                }
                else
                {
                    text = tts = "У меня нет новостей";
                }
            }
            else
            {
                text = tts = "Вы можете попросить меня прочитать последние новости спорта сказав фразу: расскажи новости";
            }

            return new AliceResponse()
            {
                Version = aliceRequest.Version,
                Response = new AliceResponseModel()
                {
                    Text = text,
                    Tts = tts
                }
            };
        }
    }
}
