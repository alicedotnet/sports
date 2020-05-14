using Microsoft.Extensions.Options;
using Sports.Alice.Models;
using Sports.Alice.Models.Settings;
using Sports.Alice.Resources;
using Sports.Alice.Services.Interfaces;
using Sports.Common.Extensions;
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
                aliceCommand = element.ToObject<AliceCommand>();
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
            else if(aliceRequest.Request.Nlu.Tokens.Contains("дальше") &&
                aliceRequest.State != null && 
                aliceRequest.State.Session is JsonElement sessionElement &&
                sessionElement.ValueKind == JsonValueKind.Object)
            {
                var state = sessionElement.ToObject<CustomSessionState>();
                aliceCommand.Type = AliceCommandType.BestComments;
                aliceCommand.Payload = state.NextNewsArticleId;
            }

            return aliceCommand.Type switch
            {
                AliceCommandType.LatestNews => GetLatestNews(aliceRequest),
                AliceCommandType.MainNews => GetMainNews(aliceRequest),
                AliceCommandType.BestComments => GetBestComments(aliceRequest, aliceCommand.Payload),
                _ => GetHelp(aliceRequest),
            };
        }

        private AliceResponseBase GetLatestNews(AliceRequest aliceRequest)
        {
            var buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel(Sports_Resources.Command_MainNews, true, new AliceCommand(AliceCommandType.MainNews), null),
                    new AliceButtonModel(Sports_Resources.Command_BestComments, true, new AliceCommand(AliceCommandType.BestComments), null)
                };
            var news = _newsService.GetLatestNews(_sportsSettings.NewsToDisplay);
            if (news.Any())
            {
                var titles = news.Select(x => x.Title);
                string text = $"{Sports_Resources.LatestNews_Header_Tts}\n\n{string.Join("\n\n", titles)}";
                string tts = $"{Sports_Resources.LatestNews_Header_Tts}{AliceTtsHelper.SilenceString500}{string.Join(AliceTtsHelper.SilenceString500, titles)}{AliceTtsHelper.SilenceString1000}{Sports_Resources.Tips_Help}";
                var response = new AliceGalleryResponse(aliceRequest, text, tts, buttons);
                response.Response.Card = new AliceGalleryCardModel
                {
                    Items = new List<AliceGalleryCardItem>(),
                    Header = new AliceGalleryCardHeaderModel(Sports_Resources.LatestNews_Header)
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
                return new AliceResponse(aliceRequest, Sports_Resources.LatestNews_NoNews, buttons);
            }
        }

        private AliceResponseBase GetMainNews(AliceRequest aliceRequest)
        {
            var buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel(Sports_Resources.Command_LatestNews, true, new AliceCommand(AliceCommandType.LatestNews), null),
                    new AliceButtonModel(Sports_Resources.Command_BestComments, true, new AliceCommand(AliceCommandType.BestComments), null)
                };
            var news = _newsService.GetPopularNews(DateTimeOffset.Now.AddDays(-1), _sportsSettings.NewsToDisplay);
            if (news.Any())
            {
                var titles = news.Select(x => x.Title);
                string text = $"{Sports_Resources.MainNews_Header_Tts}\n\n{string.Join("\n\n", titles)}";
                string tts = $"{Sports_Resources.MainNews_Header_Tts}{AliceTtsHelper.SilenceString500}{string.Join(AliceTtsHelper.SilenceString500, titles)}{AliceTtsHelper.SilenceString1000}{Sports_Resources.Tips_Help}";
                var response = new AliceGalleryResponse(aliceRequest, text, tts, buttons);
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
                return new AliceResponse(aliceRequest, Sports_Resources.MainNews_NoNews, buttons);
            }
        }

        private AliceResponseBase GetBestComments(AliceRequest aliceRequest, object payload)
        {
            var fromDate = DateTimeOffset.Now.AddDays(-1);
            NewsArticleModel newsArticle;
            if ((payload is Guid id) || (payload is JsonElement element && 
                element.ValueKind == JsonValueKind.String &&
                Guid.TryParse(element.GetString(), out id)))
            {
                newsArticle = _newsService.GetById(id);
            }
            else
            {
                newsArticle = _newsService.GetPopularNews(fromDate, 1).FirstOrDefault();
            }
            if (newsArticle != null)
            {
                var comments = _newsArticleCommentService.GetBestComments(newsArticle.Id, _sportsSettings.CommentsToDisplay);
                if (comments != null && comments.Any())
                {
                    var buttons = new List<AliceButtonModel>()
                    {
                        new AliceButtonModel()
                        {
                            Title = Sports_Resources.Command_BestComments_OpenNewsArticle,
                            Url = newsArticle.Url,
                            Hide = true
                        }
                    };
                    string ttsEnding = string.Empty;
                    var nextNewsArticle = _newsService.GetNextPopularNewsArticle(fromDate, newsArticle.Id);
                    CustomSessionState sessionState = null;
                    if (nextNewsArticle != null)
                    {
                        ttsEnding = $"{AliceTtsHelper.SilenceString500}{Sports_Resources.Tips_BestComments_Next}";
                        buttons.Add(new AliceButtonModel()
                        {
                            Title = Sports_Resources.Command_BestComments_Next,
                            Payload = new AliceCommand(AliceCommandType.BestComments, nextNewsArticle.Id),
                            Hide = true
                        });
                        sessionState = new CustomSessionState()
                        {
                            NextNewsArticleId = nextNewsArticle.Id
                        };

                    }
                    buttons.Add(new AliceButtonModel(Sports_Resources.Command_LatestNews, true, new AliceCommand(AliceCommandType.LatestNews), null));
                    buttons.Add(new AliceButtonModel(Sports_Resources.Command_MainNews, true, new AliceCommand(AliceCommandType.MainNews), null));

                    var text = new StringBuilder($"{Sports_Resources.BestComments_Title_Tts} \"{newsArticle.Title} {GetTitleEnding(newsArticle)}\":");
                    var tts = new StringBuilder($"{Sports_Resources.BestComments_Title_Tts} \"{newsArticle.Title}\"{AliceTtsHelper.SilenceString500}");
                    foreach (var comment in comments)
                    {
                        string textComment = $"\n\n{EmojiLibrary.SpeechBalloon} {comment.CommentText} {EmojiLibrary.ThumbsUp}{comment.CommentRating}";
                        string textTts = $"{AliceTtsHelper.SilenceString500}{comment.CommentText}";
                        if (text.Length + textComment.Length <= AliceResponseModel.TextMaxLenght
                            && tts.Length + textTts.Length + ttsEnding.Length <= AliceResponseModel.TtsMaxLenght)
                        {
                            text.Append(textComment);
                            tts.Append(textTts);
                        }
                    }
                    tts.Append(ttsEnding);

                    var response = new AliceResponse(aliceRequest, text.ToString(), tts.ToString(), buttons)
                    {
                        SessionState = sessionState
                    };
                    return response;
                }
            }
            var noResponseButtons = new List<AliceButtonModel>()
            {
                new AliceButtonModel(Sports_Resources.Command_LatestNews, true, new AliceCommand(AliceCommandType.LatestNews), null),
                new AliceButtonModel(Sports_Resources.Command_MainNews, true, new AliceCommand(AliceCommandType.MainNews), null)
            };
            return new AliceResponse(aliceRequest, Sports_Resources.BestComments_NoComments, noResponseButtons);
        }

        private AliceResponseBase GetHelp(AliceRequest aliceRequest)
        {
            var buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel(Sports_Resources.Command_LatestNews, false, new AliceCommand(AliceCommandType.LatestNews), null),
                    new AliceButtonModel(Sports_Resources.Command_MainNews, false, new AliceCommand(AliceCommandType.MainNews), null),
                    new AliceButtonModel(Sports_Resources.Command_BestComments, false, new AliceCommand(AliceCommandType.BestComments), null)
                };
            return new AliceResponse(aliceRequest, Sports_Resources.Help_Text_Tts, buttons);
        }

        private static string GetTitleEnding(NewsArticleModel newsArticle)
        {
            return newsArticle.IsHotContent ? $" {EmojiLibrary.FireEmoji} {newsArticle.CommentsCount}"
                : newsArticle.CommentsCount > 0 ? $" {EmojiLibrary.SpeechBalloon} {newsArticle.CommentsCount} "
                    : string.Empty;
        }
    }
}
