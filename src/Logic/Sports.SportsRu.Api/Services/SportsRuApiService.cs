using Sports.Common.Models;
using Sports.SportsRu.Api.Helpers;
using Sports.SportsRu.Api.Models;
using Sports.SportsRu.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sports.SportsRu.Api.Services
{
    public class SportsRuApiService : ISportsRuApiService
    {
        private readonly HttpClient _httpClient;

        public SportsRuApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.sports.ru")
            };
        }

        public async Task<ServiceResponse<NewsResponse>> GetNewsAsync(NewsType newsType, NewsPriority newsPriority, NewsContentOrigin newsContentOrigin, int count)
        {
            var newsRequest = new NewsRequest()
            {
                Count = count,
                Filter = new NewsRequestFilter()
            };
            switch(newsType)
            {
                case NewsType.HomePage:
                    newsRequest.Filter.Type = "homepage";
                    break;
            }
            switch(newsPriority)
            {
                case NewsPriority.Main:
                    newsRequest.Filter.NewsPriority = "main";
                    break;
            }
            switch(newsContentOrigin)
            {
                case NewsContentOrigin.Mixed:
                    newsRequest.Filter.ContentOrigin = "mixed";
                    break;
            }
            string args = HttpHelper.UrlEncodeJson(newsRequest);
            var response = await _httpClient.GetAsync($"core/news/list?args={args}").ConfigureAwait(false);
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {                
                var newsResponse = JsonSerializer.Deserialize<NewsResponse>(content);
                return ServiceResponse<NewsResponse>.Success(newsResponse);
            }
            return ServiceResponse<NewsResponse>.Error(content);
        }

        public async Task<ServiceResponse<CommentIdsResponse>> GetCommentsIdsAsync(int messageId, MessageClass messageClass, Sort sort)
        {
            var commentsIdsRequest = new CommentIdsRequest()
            {
                MessageId = messageId
            };
            switch(messageClass)
            {
                case MessageClass.News:
                    commentsIdsRequest.MessageClass = "Sports::News";
                    break;
            }
            switch(sort)
            {
                case Sort.Top10:
                    commentsIdsRequest.Sort = "top10";
                    break;
            }

            string args = HttpHelper.UrlEncodeJson(commentsIdsRequest);
            var response = await _httpClient.GetAsync($"core/api/comment/get_ids?args={args}").ConfigureAwait(false);
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var commentIdsResponse = JsonSerializer.Deserialize<CommentIdsResponse>(content);
                return ServiceResponse<CommentIdsResponse>.Success(commentIdsResponse);
            }
            return ServiceResponse<CommentIdsResponse>.Error(content);
        }
    }
}
