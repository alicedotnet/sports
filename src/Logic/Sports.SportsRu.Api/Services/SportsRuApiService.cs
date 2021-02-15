using Microsoft.Extensions.Logging;
using Sports.Common.Factories;
using Sports.Common.Models;
using Sports.SportsRu.Api.Helpers;
using Sports.SportsRu.Api.Models;
using Sports.SportsRu.Api.Resources;
using Sports.SportsRu.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly HttpClient _statHttpClient;
        private readonly ILogger<SportsRuApiService> _logger;

        public SportsRuApiService(SportsRuApiSettings sportsRuApiSettings, ILogger<SportsRuApiService> logger)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = sportsRuApiSettings.SportsRuBaseAddress
            };
            _statHttpClient = new HttpClient()
            {
                BaseAddress = sportsRuApiSettings.SportsRuStatBaseAddress
            };
            _logger = logger;
        }

        public async Task<ServiceResponse<NewsResponseCollection>> GetNewsAsync(NewsType newsType, NewsPriority newsPriority, NewsContentOrigin newsContentOrigin, int count)
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
            var uri = new Uri($"core/news/list?args={args}", UriKind.Relative);
            return await GetResponseFromSportsAsync<NewsResponseCollection>(uri).ConfigureAwait(false);
        }

        public async Task<ServiceResponse<CommentIdsResponseCollection>> GetCommentsIdsAsync(int messageId, MessageClass messageClass, Sort sort, int commentsCount = 10)
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
            var uri = new Uri($"core/api/comment/get_ids?args={args}", UriKind.Relative);
            var response = await GetResponseFromSportsAsync<CommentIdsResponseCollection>(uri).ConfigureAwait(false);
            if(response.Content != null)
            {
                response.Content = new CommentIdsResponseCollection(response.Content.Take(commentsCount));
            }

            return response;
        }

        public async Task<ServiceResponse<CommentByIdsResponse>> GetCommentsByIds(IEnumerable<int> ids)
        {
            string commentsIds = string.Join(",", ids);
            var uri = new Uri($"api/comment/get/by_ids.json?comment_ids={commentsIds}&style=newjs", UriKind.Relative);
            return await GetResponseFromSportsAsync<CommentByIdsResponse>(uri).ConfigureAwait(false);
        }

        public async Task<ServiceResponse<HotContentResponse>> GetHotContentAsync()
        {
            var uri = new Uri("api/ru/hot_content/?metod_id=1", UriKind.Relative);
            return await GetResponseFromStatAsync<HotContentResponse>(uri).ConfigureAwait(false);
        }

        private async Task<ServiceResponse<T>> GetResponseFromSportsAsync<T>(Uri requestUri)
        {
            return await GetResponseAsync<T>(requestUri, _httpClient).ConfigureAwait(false);
        }

        private async Task<ServiceResponse<T>> GetResponseFromStatAsync<T>(Uri requestUri)
        {
            return await GetResponseAsync<T>(requestUri, _statHttpClient).ConfigureAwait(false);
        }

        private async Task<ServiceResponse<T>> GetResponseAsync<T>(Uri requestUri, HttpClient httpClient)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.GetAsync(requestUri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var hotContent = JsonSerializer.Deserialize<T>(content);
                    return ServiceResponseFactory.Success(hotContent);
                }
                string errorMessage;
                if(response.StatusCode == HttpStatusCode.BadGateway)
                {
                    errorMessage = response.ReasonPhrase;
                }
                else if(response.StatusCode == HttpStatusCode.NotFound)
                {
                    errorMessage = response.ReasonPhrase;
                }
                else
                {
                    errorMessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                return ServiceResponseFactory.Error<T>(errorMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Response data: {JsonSerializer.Serialize(response)}");
                return ServiceResponseFactory.Error<T>(SportsRuApiResources.Error_Unknown);
            }
        }
    }
}
