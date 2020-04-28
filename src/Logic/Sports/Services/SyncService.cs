using Sports.Data.Context;
using Sports.Data.Entities;
using Sports.Services.Interfaces;
using Sports.SportsRu.Api.Models;
using Sports.SportsRu.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports.Services
{
    public class SyncService : ISyncService
    {
        private readonly SportsContext _sportsContext;
        private readonly ISportsRuApiService _sportsRuApiService;
        private readonly INewsService _newsService;

        public SyncService(SportsContext sportsContext, ISportsRuApiService sportsRuApiService, INewsService newsService)
        {
            _sportsContext = sportsContext;
            _sportsRuApiService = sportsRuApiService;
            _newsService = newsService;
        }

        public async Task SyncNewsAsync()
        {
            var newsResponse = await _sportsRuApiService.GetNewsAsync(NewsType.HomePage, NewsPriority.Main, NewsContentOrigin.Mixed, 100).ConfigureAwait(false);
            var hotContent = await _sportsRuApiService.GetHotContent().ConfigureAwait(false);
            var hotNews = Array.Empty<int>();
            if(hotContent.IsSuccess)
            {
                hotNews = hotContent.Content.News;
            }
            if(newsResponse.IsSuccess)
            {
                foreach (var newsArticle in newsResponse.Content)
                {
                    if(newsArticle.BodyIsEmpty ||
                        newsArticle.ContentOption?.Name == "special") //usually this is not a news article but some promotion
                    {
                        continue;
                    }
                    string idString = newsArticle.Id.ToString(CultureInfo.InvariantCulture);
                    var existingArticle = _sportsContext.NewsArticles.FirstOrDefault(x => x.ExternalId == idString);
                    if (existingArticle == null)
                    {
                        _sportsContext.NewsArticles.Add(new NewsArticle()
                        {
                            ExternalId = idString,
                            Title = newsArticle.Title,
                            Url = newsArticle.DesktopUrl,
                            IsHotContent = IsHotContent(newsArticle.Id, hotNews),
                            CommentsCount = newsArticle.CommentsCount,
                            PublishedDate = DateTimeOffset
                                .FromUnixTimeSeconds(newsArticle.Published.Timestamp)
                                .UtcDateTime
                        });
                    }
                    else
                    {
                        existingArticle.IsHotContent = IsHotContent(newsArticle.Id, hotNews);
                        existingArticle.CommentsCount = newsArticle.CommentsCount;

                        _sportsContext.NewsArticles.Update(existingArticle);
                    }
                }
                _sportsContext.SaveChanges();
            }
        }

        public async Task SyncPopularNewsCommentsAsync(DateTimeOffset fromDate, int newsCount)
        {
            var popularNews = _newsService.GetPopularNews(fromDate, newsCount);
            foreach (var newsArticle in popularNews)
            {
                if(int.TryParse(newsArticle.ExternalId, out int id))
                {
                    var commentsIdsResponse = await _sportsRuApiService.GetCommentsIdsAsync(id, MessageClass.News, Sort.Top10).ConfigureAwait(false);
                    if (commentsIdsResponse.IsSuccess)
                    {
                        var commentsByIdsResponse = await _sportsRuApiService.GetCommentsByIds(commentsIdsResponse.Content).ConfigureAwait(false);
                        if(commentsByIdsResponse.IsSuccess)
                        {
                            foreach (var comment in commentsByIdsResponse.Content.Data.Comments)
                            {
                                var existingComment = _sportsContext.NewsArticlesComments.FirstOrDefault(x => x.ExternalId == comment.Id.ToString(CultureInfo.InvariantCulture));
                                if(existingComment == null)
                                {
                                    _sportsContext.NewsArticlesComments.Add(new NewsArticleComment()
                                    {
                                        NewsArticleId = newsArticle.Id,
                                        ExternalId = comment.Id.ToString(CultureInfo.InvariantCulture),
                                        Text = NormalizeText(comment.Text),
                                        Rating = comment.Rating.Plus + comment.Rating.Minus
                                    });
                                }
                                else
                                {
                                    existingComment.Rating = comment.Rating.Plus + comment.Rating.Minus;
                                    existingComment.Text = NormalizeText(comment.Text);

                                    _sportsContext.NewsArticlesComments.Update(existingComment);
                                }
                            }
                        }
                    }
                }
            }
            _sportsContext.SaveChanges();
        }

        private string NormalizeText(string value)
        {
            return value.Replace("<br />", "\n");
        }

        private bool IsHotContent(int newsArticleId, int[] hotNews)
        {
            return hotNews.Contains(newsArticleId);
        }

        public void DeleteOldData(DateTimeOffset oldestDateToKeep)
        {
            var date = oldestDateToKeep.UtcDateTime;
            var oldArticles = _sportsContext.NewsArticles
                .Where(x => x.PublishedDate < date).ToArray();
            _sportsContext.NewsArticles.RemoveRange(oldArticles);

            _sportsContext.SaveChanges();
        }
    }
}
