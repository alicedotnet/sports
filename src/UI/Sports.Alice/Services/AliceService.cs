using Microsoft.Extensions.Options;
using Sports.Alice.Models;
using Sports.Alice.Models.Settings;
using Sports.Alice.Services.Interfaces;
using Sports.Models;
using Sports.Services.Interfaces;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Yandex.Alice.Sdk.Helpers;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Services
{
    public class AliceService : IAliceService
    {
        private readonly INewsService _newsService;
        private readonly INewsArticleCommentService _newsArticleCommentService;
        private readonly SportsSettings _sportsSettings;

        public AliceService(INewsService newsService, INewsArticleCommentService newsArticleCommentService, IOptions<SportsSettings> sportsSettings)
        {
            if (sportsSettings == null)
            {
                throw new ArgumentNullException(nameof(sportsSettings));
            }
            _newsService = newsService;
            _newsArticleCommentService = newsArticleCommentService;
            _sportsSettings = sportsSettings.Value;
        }

        public AliceResponseBase ProcessRequest(AliceRequest aliceRequest)
        {
            if (aliceRequest == null)
            {
                throw new ArgumentNullException(nameof(aliceRequest));
            }

            AliceCommand aliceCommand = new AliceCommand();
            if (aliceRequest.Session.New)
            {
                aliceCommand.Type = AliceCommandType.LatestNews;
            }
            else if (aliceRequest.Request.Type == AliceRequestType.ButtonPressed &&
                aliceRequest.Request.Payload is JsonElement element &&
                element.ValueKind == JsonValueKind.Object)
            {
                var bufferWriter = new ArrayBufferWriter<byte>();
                using (var writer = new Utf8JsonWriter(bufferWriter))
                    element.WriteTo(writer);
                aliceCommand = JsonSerializer.Deserialize<AliceCommand>(bufferWriter.WrittenSpan);
            }
            else if (aliceRequest.Request.Nlu.Tokens.Contains("новости"))
            {
                if (aliceRequest.Request.Nlu.Tokens.Contains("главные") || aliceRequest.Request.Nlu.Tokens.Contains("популярные"))
                {
                    aliceCommand.Type = AliceCommandType.MainNews;
                }
                else
                {
                    aliceCommand.Type = AliceCommandType.LatestNews;
                }
            }
            else if (aliceRequest.Request.Nlu.Tokens.Contains("комментарии")
                || aliceRequest.Request.Nlu.Tokens.Contains("комментарий"))
            {
                aliceCommand.Type = AliceCommandType.BestComments;
            }

            switch (aliceCommand.Type)
            {
                case AliceCommandType.LatestNews:
                    return GetLatestNews(aliceRequest);
                case AliceCommandType.MainNews:
                    return GetMainNews(aliceRequest);
                case AliceCommandType.BestComments:
                    return GetBestComments(aliceRequest, aliceCommand.Payload);
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
                        Title = AliceCommandsTitle.MainNews,
                        Payload = new AliceCommand(AliceCommandType.MainNews),
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
                        Title = AliceCommandsTitle.LatestNews,
                        Payload = new AliceCommand(AliceCommandType.LatestNews),
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

        private AliceResponseBase GetBestComments(AliceRequest aliceRequest, object payload)
        {
            var fromDate = DateTimeOffset.Now.AddDays(-1);
            NewsArticleModel newsArticle;
            if (payload is JsonElement element && 
                element.ValueKind == JsonValueKind.String &&
                Guid.TryParse(element.GetString(), out Guid id))
            {
                newsArticle = _newsService.GetById(id);
            }
            else
            {
                newsArticle = _newsService.GetPopularNews(fromDate, 1).FirstOrDefault();
            }
            var comments = _newsArticleCommentService.GetBestComments(newsArticle.Id, _sportsSettings.CommentsToDisplay);
            if (comments != null && comments.Any())
            {
                var buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel()
                    {
                        Title = "к новости",
                        Url = newsArticle.Url,
                        Hide = true
                    }
                };
                string ttsEnding = string.Empty;
                var nextNewsArticle = _newsService.GetNextPopularNewsArticle(fromDate, newsArticle.Id);
                if (nextNewsArticle != null)
                {
                    ttsEnding = $" {TtsHelper.GetSilenceString(500)} для перехода к следующий новости скажите: дальше";
                    buttons.Add(new AliceButtonModel()
                    {
                        Title = "дальше",
                        Payload = new AliceCommand(AliceCommandType.BestComments, nextNewsArticle.Id),
                        Hide = true
                    });
                }
                buttons.Add(new AliceButtonModel()
                {
                    Title = AliceCommandsTitle.LatestNews,
                    Payload = new AliceCommand(AliceCommandType.LatestNews),
                    Hide = true
                });
                buttons.Add(new AliceButtonModel()
                {
                    Title = AliceCommandsTitle.MainNews,
                    Payload = new AliceCommand(AliceCommandType.MainNews),
                    Hide = true
                });

                var text = new StringBuilder($"Лучшие комментарии под новостью \"{newsArticle.Title} {GetTitleEnding(newsArticle)}\":");
                var tts = new StringBuilder($"Лучшие комментарии под новостью \"{newsArticle.Title}\": sil <[500]> ");
                foreach (var comment in comments)
                {
                    string textComment = $"\n\n{EmojiLibrary.SpeechBalloon} {comment.CommentText} {EmojiLibrary.ThumbsUp}{comment.CommentRating}";
                    string textTts = $" sil <[500]>  {comment.CommentText}";
                    if (text.Length + textComment.Length <= AliceResponseModel.TextMaxLenght
                        && tts.Length + textTts.Length + ttsEnding.Length <= AliceResponseModel.TtsMaxLenght)
                    {
                        text.Append(textComment);
                        tts.Append(textTts);
                    }
                }
                tts.Append(ttsEnding);

                var response = new AliceResponse(aliceRequest, text.ToString(), tts.ToString(), buttons);
                return response;
            }
            else
            {
                var buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel()
                    {
                        Title = AliceCommandsTitle.LatestNews,
                        Payload = new AliceCommand(AliceCommandType.LatestNews),
                        Hide = true
                    },
                    new AliceButtonModel()
                    {
                        Title = AliceCommandsTitle.MainNews,
                        Payload = new AliceCommand(AliceCommandType.MainNews),
                        Hide = true
                    }
                };
                return new AliceResponse(aliceRequest, "У меня нет лучших комментариев", buttons);
            }
        }

        private AliceResponseBase GetHelp(AliceRequest aliceRequest)
        {
            var buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel()
                    {
                        Title = AliceCommandsTitle.LatestNews,
                        Payload = new AliceCommand(AliceCommandType.LatestNews)
                    },
                    new AliceButtonModel()
                    {
                        Title = AliceCommandsTitle.MainNews,
                        Payload = new AliceCommand(AliceCommandType.MainNews)
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
