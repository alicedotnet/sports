using Sports.Alice.Models;
using Sports.Alice.Models.Settings;
using Sports.Alice.Resources;
using Sports.Data.Models;
using Sports.Models;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Helpers;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Scenes
{
    public class BestCommentsScene : Scene
    {
        public override SceneType SceneType => SceneType.BestComments;


        private readonly INewsService _newsService;
        private readonly SportsSettings _sportsSettings;
        private readonly INewsArticleCommentService _newsArticleCommentService;

        public BestCommentsScene(INewsService newsService
            , SportsSettings sportsSettings
            , INewsArticleCommentService newsArticleCommentService)
        {
            _newsService = newsService;
            _sportsSettings = sportsSettings;
            _newsArticleCommentService = newsArticleCommentService;
        }

        public override Scene MoveToNextScene(SportsRequest sportsRequest)
        {
            if(sportsRequest.Request.Nlu.Intents.Next != null)
            {
                return this;
            }
            return null;
        }

        public override IAliceResponseBase Repeat(SportsRequest sportsRequest)
        {
            return Reply(sportsRequest);
        }

        public override IAliceResponseBase Reply(SportsRequest sportsRequest)
        {
            SportsResponse response;
            var fromDate = DateTimeOffset.Now.AddDays(-1);
            NewsArticleModel newsArticle = null;
            if (sportsRequest.State.Session.NextNewsArticleId != Guid.Empty)
            {
                newsArticle = _newsService.GetById(sportsRequest.State.Session.NextNewsArticleId);
            }

            if(newsArticle == null)
            {
                newsArticle = _newsService.GetPopularNews(fromDate, new PagedRequest(1, 0)).Items.FirstOrDefault();
            }
            IEnumerable<NewsArticleCommentModel> comments = null;
            if (newsArticle != null)
            {
                comments = _newsArticleCommentService.GetBestComments(newsArticle.Id, _sportsSettings.CommentsToDisplay);
            }

            if (comments != null && comments.Any())
            {
                var buttons = new List<AliceButtonModel>()
                    {
                        new AliceButtonModel()
                        {
                            Title = Sports_Resources.Command_BestComments_OpenNewsArticle,
                            Url = newsArticle.Url,
                            Hide = false
                        }
                    };
                string ttsEnding = string.Empty;
                var nextNewsArticle = _newsService.GetNextPopularNewsArticle(fromDate, newsArticle.Id);
                Guid nextNewsArticleId = Guid.Empty;
                if (nextNewsArticle != null)
                {
                    ttsEnding = $"{AliceHelper.SilenceString500}{Sports_Resources.Tips_BestComments_Next}";
                    buttons.Add(new SportsButtonModel(Sports_Resources.Command_BestComments_Next));
                    nextNewsArticleId = nextNewsArticle.Id;
                }
                buttons.Add(new SportsButtonModel(Sports_Resources.Command_LatestNews));
                buttons.Add(new SportsButtonModel(Sports_Resources.Command_MainNews));

                var text = new StringBuilder($"{Sports_Resources.BestComments_Title_Tts} \"{newsArticle.Title} {GetTitleEnding(newsArticle)}\":");
                var tts = new StringBuilder($"{Sports_Resources.BestComments_Title_Tts} \"{newsArticle.Title}\"{AliceHelper.SilenceString500}");
                foreach (var comment in comments)
                {
                    string textComment = $"\n\n{EmojiLibrary.SpeechBalloon} {comment.CommentText} {EmojiLibrary.ThumbsUp}{comment.CommentRating}";
                    string textTts = $"{AliceHelper.SilenceString500}{comment.CommentText}";
                    if (text.Length + textComment.Length <= AliceResponseModel.TextMaxLenght
                        && tts.Length + textTts.Length + ttsEnding.Length <= AliceResponseModel.TtsMaxLenght)
                    {
                        text.Append(textComment);
                        tts.Append(textTts);
                    }
                }
                tts.Append(ttsEnding);

                response = new SportsResponse(sportsRequest, text.ToString(), tts.ToString(), buttons);
                response.SessionState.NextNewsArticleId = nextNewsArticleId;
                response.SessionState.CurrentScene = SceneType;
            }
            else
            {
                var noResponseButtons = new List<AliceButtonModel>()
                {
                    new SportsButtonModel(Sports_Resources.Command_LatestNews),
                    new SportsButtonModel(Sports_Resources.Command_MainNews)
                };
                response = new SportsResponse(sportsRequest, Sports_Resources.BestComments_NoComments, noResponseButtons);
            }
            response.SessionState.CurrentScene = SceneType;
            return response;
        }
    }
}
